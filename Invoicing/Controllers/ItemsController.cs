using FluentValidation;
using Invoicing.DTOs;
using Invoicing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;

namespace Invoicing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        IItemService<ItemDTO, ItemInsertDTO> _itemService;
        IValidator<ItemInsertDTO> _itemInsertValidator; 

        public ItemsController([FromKeyedServices("ItemService")] IItemService<ItemDTO, ItemInsertDTO> itemService,
            IValidator<ItemInsertDTO> itemInsertValidator)
        {
            _itemService = itemService;
            _itemInsertValidator = itemInsertValidator;
        }

        [HttpGet("{hash}")]
        public async Task<IEnumerable<ItemDTO>> GetAllByHash(string hash) => await _itemService.GetAllByHash(hash);

        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetByHash(string hash) => await _itemService.GetAllByHash(hash);

        [HttpPost] // Fix arg conversion to ENUMERABLE
        public async Task<ActionResult<IEnumerable<ItemDTO>>> Add(JsonElement itemsInsertDTO)
        {
            IEnumerable<ItemInsertDTO> input;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (itemsInsertDTO.ValueKind == JsonValueKind.Array)
            {
                input = JsonSerializer.Deserialize<IEnumerable<ItemInsertDTO>>(itemsInsertDTO.GetRawText(), options)!;
            }
            else if (itemsInsertDTO.ValueKind == JsonValueKind.Object)
            {
                var singleObject = JsonSerializer.Deserialize<ItemInsertDTO>(itemsInsertDTO.GetRawText(), options);
                input = new List<ItemInsertDTO> { singleObject! };
            }
            else
            {
                return BadRequest("Invalid input type.");
            }

            input.Select(async i =>
            {
                var validationResult = await _itemInsertValidator.ValidateAsync(i);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                return null;
            });

            if (!_itemService.Validate(input))
            {
                return BadRequest(_itemService.Errors);
            }

            var itemsDTOS = await _itemService.Add(input);

            return CreatedAtAction(nameof(GetByHash), itemsDTOS.Select(i => new { hash = i.Hash }), itemsDTOS);
        }
    }
}
