using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using ShopBridgeItemTrackerAPI.Dtos;
using ShopBridgeItemTrackerAPI.Entities;
using ShopBridgeItemTrackerAPI.Repository.Interface;
using System;
using System.IO;
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
        private readonly IWebHostEnvironment _env;
        public AddItemCommandHandler(IItemRepository itemRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _env = env;
        }
        public async Task<int> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
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
            }

            Item item = _mapper.Map<Item>(request.ItemDto);
            item.ImgURL = fileName;

            return await _itemRepository.AddUpdateItemAsync(item);
        }
    }
}
