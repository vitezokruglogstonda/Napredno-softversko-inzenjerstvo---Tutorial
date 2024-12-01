using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Tutorial.Models;
using Tutorial.Models.Database;
using Microsoft.IdentityModel.Tokens;
using Tutorial.Services.CacheService;
using System.Security.Claims;
using Tutorial.Models.Response;
using Tutorial.Models.Requests;
using Tutorial.Exceptions;
using System.Security.AccessControl;

namespace Tutorial.Services.AccountService
{
    public class AccountService: IAccountService
    {
        private readonly CustomDbContext _dbContext;
        private readonly ICacheService _cacheService;
        public JwtSettings _jwtSettings { get; set; }
        public AccountService(
            CustomDbContext DbContext, 
            IOptions<JwtSettings> JwtSettings,
            ICacheService CacheService
            )
        {
            _dbContext = DbContext;
            _jwtSettings = JwtSettings.Value;
            _cacheService = CacheService;
        }
        public async Task<User> GetUserById(int id)
        {
            return await _dbContext.Users
                .Where(user => user.Id == id)
                .Include(user => user.Items)
                .FirstOrDefaultAsync() ?? throw new CustomException(400, "User doesn't exist.");
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users
                .Include(user => user.Items)
                .FirstOrDefaultAsync(x => x.Email == email) ?? throw new CustomException(400, "User doesn't exist.");
        }
        private void HashPassword(out byte[] hash, out byte[] salt, string passwordString)
        {
            HMACSHA512 hashObj = new HMACSHA512();
            salt = hashObj.Key;
            byte[] password = Encoding.UTF8.GetBytes(passwordString);
            hash = hashObj.ComputeHash(password);
        }
        public bool CheckPassword(string email, string passwordString)
        {
            User? user = _dbContext.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user == null)
                return false;

            HMACSHA512 hashObj = new HMACSHA512(user.PasswordSalt!);
            byte[] password = Encoding.UTF8.GetBytes(passwordString);
            byte[] hash = hashObj.ComputeHash(password);

            int len = hash.Length;
            for (int i = 0; i < len; i++)
            {
                if (user.PasswordHash![i] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
        public async Task CheckJwtToken(HttpContext httpContext, string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(10)
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

                int userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                if (jwtToken.ValidTo < DateTime.UtcNow)
                    return;

                User? user = await _cacheService.GetUser(userId);

                if (user != null && user.OnlineStatus)
                {
                    httpContext.Items["User"] = user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void GenerateJwtToken(User user, HttpContext httpContext)
        {
            TimeSpan tokenLifetime = TimeSpan.Parse(_jwtSettings.AccessTokenLifetime);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.Add(tokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            httpContext.Response.Headers["JWT"] = token;
        }
        public async Task<UserResponse> LogUserIn(User user, HttpContext httpContext)
        {
            user!.OnlineStatus = true;
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
            await _cacheService.StoreUserToCache(user!);
            GenerateJwtToken(user, httpContext);
            return new UserResponse(user);
        }
        public async Task<UserResponse> TryLogUserIn(LogInRequest loginRequest, HttpContext httpContext)
        {
            if (!CheckIfEmailExists(loginRequest.Email!))
                throw new CustomException(400, "Wrong email address.");
            if (!CheckPassword(loginRequest.Email!, loginRequest.Password!))
                throw new CustomException(400, "Wrong password.");
            User user = await GetUserByEmail(loginRequest.Email!);
            return await LogUserIn(user, httpContext);
        }
        public async Task<bool> LogUserOut(User user)
        {
            if (user == null)
                throw new CustomException(400, "Invalid token or user doesn't exist.");
            user.OnlineStatus = false;
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
            await _cacheService.RemoveUserFromCache(user.Id);
            return true;
        }
        public async Task<UserResponse> AddNewUser(RegisterRequest registerRequest, HttpContext httpContext)
        {
            if (CheckIfEmailExists(registerRequest.Email!))
                throw new CustomException(400, "User with this email already exists.");

            if (String.IsNullOrEmpty(registerRequest.Password))
                throw new CustomException(400, "Invalid password.");

            byte[] hash, salt;
            HashPassword(out hash, out salt, registerRequest.Password);

            User user = new User(
                registerRequest.Email!,
                hash,
                salt,
                UserType.Regular,
                false
            );

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return await LogUserIn(user, httpContext);
        }
        public bool CheckIfEmailExists(string email)
        {
            return _dbContext.Users.Any(u => string.Equals(u.Email, email));
        }
        public async Task CreateAdmin()
        {
            User? admin = await _dbContext.Users
                .Where(user => user.UserType == UserType.Admin).FirstOrDefaultAsync();

            if (admin != null) throw new CustomException(403, "Admin already exists.");

            byte[] hash, salt;
            HashPassword(out hash, out salt, "admin");

            admin = new User(
                "admin",
                hash,
                salt,
                UserType.Admin,
                false
            );

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.SaveChangesAsync();
        }
    }
}
