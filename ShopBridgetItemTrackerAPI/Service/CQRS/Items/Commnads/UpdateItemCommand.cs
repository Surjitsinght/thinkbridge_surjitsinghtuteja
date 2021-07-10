using AutoMapper;
using MediatR;
using ShopBridgeItemTrackerAPI.Dtos;
using ShopBridgeItemTrackerAPI.Entities;
using ShopBridgeItemTrackerAPI.Repository.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridgeItemTrackerAPI.Service.CQRS.Items.Commnads
{
    public class UpdateItemCommand : IRequest<int>
    {
        public int Id { get; set; }
        public AddEditItemDto ItemDto { get; init; }
    }

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, int>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        public UpdateItemCommandHandler(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {            
            var existingItem = _itemRepository.GetItemAsync(request.Id);
            if (existingItem is null) return 0;

            Item item = _mapper.Map<Item>(request.ItemDto);
            item.Id = request.Id;
            return await _itemRepository.AddUpdateItemAsync(item);
        }
    }
}
