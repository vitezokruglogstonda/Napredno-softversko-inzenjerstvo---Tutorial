using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using Tutorial.Exceptions;
using Tutorial.Models.Database;
using Tutorial.Services.AccountService;

namespace Tutorial.Services.CacheService
{
    public class CacheService: ICacheService
    {
        private readonly IDatabase _redis;
        private readonly CustomDbContext _dbContext;
        public CacheService(
            IConnectionMultiplexer Redis,
            CustomDbContext DbContext
            )
        {
            _redis = Redis.GetDatabase();
            _dbContext = DbContext;
        }
        public async Task<User?> GetUser(int userId)
        {
            User? user = null;
            string? serializedUser;

            serializedUser = await _redis.StringGetAsync("user_cache_" + userId.ToString());
            if (serializedUser != null)
            {
                user = JsonSerializer.Deserialize<User>(serializedUser)!;
            }
            else
            {
                user = await _dbContext.Users
                    .Where(user => user.Id == userId)
                    .Include(user => user.Items)
                    .FirstOrDefaultAsync() ?? throw new CustomException(400, "User doesn't exist.");
                await _redis.StringSetAsync("user_cache_" + userId.ToString(), JsonSerializer.Serialize<User>(user), TimeSpan.FromMinutes(30));
            }

            return user;
        }

        public async Task StoreUserToCache(User user)
        {
            await _redis.StringSetAsync("user_cache_" + user.Id.ToString(), JsonSerializer.Serialize<User>(user), TimeSpan.FromMinutes(30));
        }

        public async Task RemoveUserFromCache(int userId)
        {
            await _redis.KeyDeleteAsync("user_cache_" + userId.ToString());
        }
    }
}
