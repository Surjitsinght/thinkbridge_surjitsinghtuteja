using ShopBridgeItemTrackerAPI.Entities;
using ShopBridgeItemTrackerAPI.Service.Mappings;

namespace ShopBridgeItemTrackerAPI.Dtos
{
    public class ItemDto : IMapFrom<Item>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }        
    }
}
