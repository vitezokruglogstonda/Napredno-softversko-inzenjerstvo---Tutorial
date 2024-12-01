using Tutorial.Models.Database;
using Tutorial.Models.Requests;

namespace Tutorial.Services.ItemService
{
    public interface IItemService
    {
        public Task<List<Item>> GetAllItems();
        public Task<List<Item>> GetAllUsersItems(int userId);
        public Task<Item> GetItem(int itemId);
        public Task<Item> AddItem(User user, CreateItemRequest itemRequest);
        public Task<Item> ChangeItem(User user, ChangeItemRequest itemRequest);
        public Task DeleteItem(User user, int itemId);
        public Task DeleteAllItems();
    }
}
