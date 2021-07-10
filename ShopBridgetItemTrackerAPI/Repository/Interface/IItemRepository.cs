using ShopBridgeItemTrackerAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridgeItemTrackerAPI.Repository.Interface
{
    public interface IItemRepository
    {
        Task<int> AddUpdateItemAsync(Item item);
        Task<List<Item>> GetItemsAsync();
        Task<Item> GetItemAsync(int id);
        Task<bool> DeleteItemAsync(int id);
    }
}
