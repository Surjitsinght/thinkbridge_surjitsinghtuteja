using ShopBridgeItemTrackerAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridgeItemTrackerAPI.Repository.Interface
{
    public interface IItemRepository
    {
        Task<int> AddUpdateItemAsync(Item item);        
        Task<List<Items>> GetItemsAsync(string keyword, int? pageNo, int? pageSize, string sortField, string sortExp);
        Task<Items> GetItemAsync(int id);
        Task<bool> DeleteItemAsync(int id);
        
    }
}
