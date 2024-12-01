using Tutorial.Models.Database;

namespace Tutorial.Services.CacheService
{
    public interface ICacheService
    {
        public Task<User?> GetUser(int userId);
        public Task StoreUserToCache(User user);
        public Task RemoveUserFromCache(int userId);
    }
}
