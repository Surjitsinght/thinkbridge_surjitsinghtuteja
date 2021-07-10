using AutoMapper;
using MediatR;
using ShopBridgeItemTrackerAPI.Dtos;
using ShopBridgeItemTrackerAPI.Entities;
using ShopBridgeItemTrackerAPI.Repository.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridgeItemTrackerAPI.Service.CQRS.Items.Commnads
{
    public class AddItemCommand : IRequest<int>
    {
        public AddEditItemDto ItemDto { get; init; }
    }

    public class AddItemCommandHandler : IRequestHandler<AddItemCommand, int>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        public AddItemCommandHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            Item item = _mapper.Map<Item>(request.ItemDto);
            return await _itemRepository.AddUpdateItemAsync(item);
        }
    }
}
