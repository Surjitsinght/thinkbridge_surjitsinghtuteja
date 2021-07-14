using MediatR;
using Microsoft.AspNetCore.Hosting;
using ShopBridgeItemTrackerAPI.Repository.Interface;
using System.IO;
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
        private readonly IWebHostEnvironment _env;
        public DeleteItemCommandHandler(IItemRepository itemRepository, IWebHostEnvironment env)
        {
            _itemRepository = itemRepository;
            _env = env;
        }
        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var existingItem = _itemRepository.GetItemAsync(request.Id);
            if (existingItem is null) return false;

            var isDeleted = await _itemRepository.DeleteItemAsync(request.Id);

            if (isDeleted)
            {
                if (string.IsNullOrEmpty(existingItem.Result.ImgURL) == false)
                {
                    var localUploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                    string oldOne = Path.Combine(localUploadsPath, Path.GetFileName(existingItem.Result.ImgURL));
                    File.Delete(oldOne);
                }
            }
            return isDeleted;
        }
    }
}
