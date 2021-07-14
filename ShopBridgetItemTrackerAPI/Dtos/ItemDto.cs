using AutoMapper;
using ShopBridgeItemTrackerAPI.Entities;
using ShopBridgeItemTrackerAPI.Service.Mappings;
using System;

namespace ShopBridgeItemTrackerAPI.Dtos
{
    public class ItemDto : IMapFrom<Items>
    {
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public string ImgURL { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public void Mapping(Profile profile)
        {            
            profile.CreateMap<Items, ItemDto>()
                .ForMember(d => d.ImgURL, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.ImgURL) ? "/img/no-image.png" : s.ImgURL));
        }
    }
}
