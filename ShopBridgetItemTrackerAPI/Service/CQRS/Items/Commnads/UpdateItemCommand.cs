using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using ShopBridgeItemTrackerAPI.Dtos;
using ShopBridgeItemTrackerAPI.Repository.Interface;
using System;
using System.IO;
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
        private readonly IWebHostEnvironment _env;

        public UpdateItemCommandHandler(IItemRepository itemRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _env = env;
        }
        public async Task<int> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var existingItem = _itemRepository.GetItemAsync(request.Id);
            if (existingItem is null) return 0;

            string fileName = null;
            if (request.ItemDto.ProductImg?.Length > 0)
            {
                var localUploadsPath = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(localUploadsPath))
                    Directory.CreateDirectory(localUploadsPath);

                fileName = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + "-" + request.ItemDto.ProductImg.FileName;

                string filePath = Path.Combine(localUploadsPath, fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ItemDto.ProductImg.CopyToAsync(fileStream);
                    fileStream.Flush();
                }

                if (string.IsNullOrEmpty(existingItem.Result.ImgURL) == false)
                {
                    string oldOne = Path.Combine(localUploadsPath, Path.GetFileName(existingItem.Result.ImgURL));
                    File.Delete(oldOne);
                }
            }

            Entities.Item item = _mapper.Map<Entities.Item>(request.ItemDto);
            item.ImgURL = fileName;
            item.Id = request.Id;
            return await _itemRepository.AddUpdateItemAsync(item);
        }
    }
}
