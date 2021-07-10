using AutoMapper;
using MediatR;
using ShopBridgeItemTrackerAPI.Dtos;
using ShopBridgeItemTrackerAPI.Repository.Interface;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridgeItemTrackerAPI.Service.CQRS.Items.Queries
{
    public class GetItemsQuery : IRequest<List<ItemDto>> { }
    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, List<ItemDto>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public GetItemsQueryHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        public async Task<List<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var result = await _itemRepository.GetItemsAsync();
            return _mapper.Map<List<ItemDto>>(result);
        }
    }
}
