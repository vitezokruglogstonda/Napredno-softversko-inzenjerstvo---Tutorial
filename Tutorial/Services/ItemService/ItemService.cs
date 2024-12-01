using Microsoft.EntityFrameworkCore;
using Tutorial.Exceptions;
using Tutorial.Models;
using Tutorial.Models.Database;
using Tutorial.Models.Requests;

namespace Tutorial.Services.ItemService
{
    public class ItemService: IItemService
    {
        private readonly CustomDbContext _dbContext;
        public ItemService(CustomDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<Item> AddItem(User user, CreateItemRequest itemRequest)
        {
            if (itemRequest == null || String.IsNullOrEmpty(itemRequest.Title) || String.IsNullOrEmpty(itemRequest.Description)) throw new CustomException(400, "Invalid item object.");
            Item item = new Item(itemRequest, user.Id);
            item.Owner = user;
            if(user.Items == null) user.Items = new();
            user.Items.Add(item);
            _dbContext.Items.Add(item);
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<Item> ChangeItem(User user, ChangeItemRequest itemRequest)
        {
            if (itemRequest == null || (String.IsNullOrEmpty(itemRequest.Title) && String.IsNullOrEmpty(itemRequest.Description))) throw new CustomException(400, "Invalid item object.");
            Item item = await _dbContext.Items
                .Where(item => item.Id == itemRequest.Id && item.OwnerId == user.Id)
                .FirstOrDefaultAsync() ?? throw new CustomException(400, "No item found, or it doesn'g belong to you.");

            item.Title = itemRequest.Title ?? item.Title;
            item.Description = itemRequest.Description ?? item.Description;

            _dbContext.Update(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAllItems()
        {
            List<User> users = await _dbContext.Users
                .Where(user => user.UserType != UserType.Admin)
                .Include(user => user.Items)
                .ToListAsync();

            if (users.Count == 0) throw new CustomException(200, "There were no users.");

            foreach (User user in users)
            {
                if(user.Items == null) continue;
                foreach (Item item in user.Items)
                {
                    _dbContext.Remove(item);
                }
                user.Items.Clear();
                _dbContext.Update(user);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteItem(User user, int itemId)
        {
            Item item = await _dbContext.Items
                .Where(item => item.Id == itemId && item.OwnerId == user.Id)
                .FirstOrDefaultAsync() ?? throw new CustomException(400, "No item found, or it doesn'g belong to you.");

            user.Items!.Remove(item);

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Item>> GetAllItems()
        {
            return await _dbContext.Items.ToListAsync();
        }

        public async Task<List<Item>> GetAllUsersItems(int userId)
        {
            return await _dbContext.Items
                .Where(item => item.OwnerId == userId)
                .ToListAsync();
        }

        public async Task<Item> GetItem(int itemId)
        {
            return await _dbContext.Items.FindAsync(itemId) ?? throw new CustomException(400, "No item with given ID found.");
        }
    }
}
