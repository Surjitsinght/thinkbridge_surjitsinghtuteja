using AutoMapper;

namespace ShopBridgeItemTrackerAPI.Service.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
