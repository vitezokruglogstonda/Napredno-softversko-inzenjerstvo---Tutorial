using Tutorial.Models.Database;
using Tutorial.Models.Requests;
using Tutorial.Models.Response;

namespace Tutorial.Services.AccountService
{
    public interface IAccountService
    {
        public bool CheckPassword(string email, string passwordString);
        public Task CheckJwtToken(HttpContext httpContext, string token);
        public void GenerateJwtToken(User user, HttpContext httpContext);
        public Task<User> GetUserById(int id);
        public Task<User> GetUserByEmail(string email);
        public Task<UserResponse> LogUserIn(User user, HttpContext httpContext);
        public Task<UserResponse> TryLogUserIn(LogInRequest loginRequest, HttpContext httpContext);
        public Task<bool> LogUserOut(User user);
        public Task<UserResponse> AddNewUser(RegisterRequest registerRequest, HttpContext httpContext);
        public bool CheckIfEmailExists(string email);
        public Task CreateAdmin();
    }
}
