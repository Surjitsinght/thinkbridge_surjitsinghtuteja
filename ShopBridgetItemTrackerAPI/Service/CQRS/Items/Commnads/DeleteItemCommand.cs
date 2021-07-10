using MediatR;
using ShopBridgeItemTrackerAPI.Repository.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridgeItemTrackerAPI.Service.CQRS.Items.Commnads
{
    public class DeleteItemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
    {
        private readonly IItemRepository _itemRepository;        
        public DeleteItemCommandHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;            
        }
        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {            
            var existingItem = _itemRepository.GetItemAsync(request.Id);
            if (existingItem is null) return false;
            
            return await _itemRepository.DeleteItemAsync(request.Id);
        }
    }
}
