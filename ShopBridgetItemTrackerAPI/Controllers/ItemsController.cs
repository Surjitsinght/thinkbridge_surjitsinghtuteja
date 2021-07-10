using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridgeItemTrackerAPI.Dtos;
using ShopBridgeItemTrackerAPI.Service.CQRS.Items.Commnads;
using ShopBridgeItemTrackerAPI.Service.CQRS.Items.Queries;
using System;
using System.Threading.Tasks;

namespace ShopBridgeItemTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetItems()
        {
            try
            {
                var result = await _mediator.Send(new GetItemsQuery());
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetItemQuery { Id = id });

                if (result is null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateItem(AddEditItemDto item)
        {
            try
            {
                if (item is null)
                    return BadRequest();

                var result = await _mediator.Send(new AddItemCommand { ItemDto = item });
                return CreatedAtAction(nameof(GetItem), new { id = result }, item);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       "Error while creating new item record");
            }

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateItem(int id, AddEditItemDto item)
        {
            try
            {
                var result = await _mediator.Send(new UpdateItemCommand { Id = id, ItemDto = item });
                if (result == 0)
                {
                    return NotFound($"Item with id - {id} not found");
                }
                return Ok($"Item with id - {id} updated");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                          "Error while updating item record");
            }
            
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteItemCommand { Id = id });
                if (result == false)
                {
                    return NotFound($"Item with id - {id} not found");
                }
                return Ok($"Item with id - {id} deleted");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                         "Error while deleting item record");
            }            
        }
    }
}
