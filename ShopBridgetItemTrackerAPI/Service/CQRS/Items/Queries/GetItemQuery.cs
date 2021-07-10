using AutoMapper;
using MediatR;
using ShopBridgeItemTrackerAPI.Dtos;
using ShopBridgeItemTrackerAPI.Repository.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridgeItemTrackerAPI.Service.CQRS.Items.Queries
{
    public class GetItemQuery : IRequest<ItemDto>
    {
        public int Id { get; set; }
    }
    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, ItemDto>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public GetItemQueryHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        public async Task<ItemDto> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            var result = await _itemRepository.GetItemAsync(request.Id);

            if (result is null) return null;

            return _mapper.Map<ItemDto>(result);
        }
    }
}
