using ShopBridgeItemTrackerAPI.Dtos;
using ShopBridgeItemTrackerAPI.Service.Mappings;

namespace ShopBridgeItemTrackerAPI.Entities
{
    public record Item : IMapFrom<AddEditItemDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
