using ShopBridgeItemTrackerAPI.Dtos;
using ShopBridgeItemTrackerAPI.Service.Mappings;
using System;

namespace ShopBridgeItemTrackerAPI.Entities
{
    public class Item : IMapFrom<AddEditItemDto>
    {
        public int Id { get; set; }
        public string ImgURL { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class Items : Item
    {
        public int TotalRows { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
