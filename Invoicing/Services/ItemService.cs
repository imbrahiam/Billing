using AutoMapper;
using Invoicing.DTOs;
using Invoicing.Models;
using Invoicing.Repository;

namespace Invoicing.Services
{
    public class ItemService : IItemService<ItemDTO, ItemInsertDTO>
    {
        public List<string> Errors { get; }
        private IItemRepository<Item> _itemRepository;
        private IMapper _mapper;

        public ItemService(IItemRepository<Item> itemRepository, IMapper mapper)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
            Errors = new List<string>();
        }

        public async Task<IEnumerable<ItemDTO>> GetAllByHash(string hash)
        {
            var items = await _itemRepository.GetAllByHash(hash);

            return items.Select(i => _mapper.Map<ItemDTO>(i));
        }

        public async Task<IEnumerable<ItemDTO>> Add(IEnumerable<ItemInsertDTO> entitiesInsertDTO)
        {
            var items = entitiesInsertDTO.Select(i => _mapper.Map<Item>(i));

            await _itemRepository.Add(items);
            await _itemRepository.Save();

            var itemsDTOs = items.Select(i => _mapper.Map<ItemDTO>(i));
            itemsDTOs = itemsDTOs.Select(i =>
            {
                _itemRepository.SearchProduct(p =>
                {
                    if (p.ProductId == i.ProductId)
                    {
                        i.ProductName = p.Name;
                    }

                    return true;
                });

                return i;
            });

            return itemsDTOs;
        }

        public bool Validate(IEnumerable<ItemInsertDTO> entitiesInsertDTO)
        {
            var validHash = entitiesInsertDTO.Any(e =>
            {
                if (!(_itemRepository.SearchInvoice(i => i.Hash == e.Hash).Count() > 0))
                {
                    Errors.Add("Invalid invoice hash");
                    return false;
                }

                return true;
            });

            var validProductsIDS = entitiesInsertDTO.All(entityInsertDTO =>
            {
                if (!(_itemRepository.SearchProduct(p => p.ProductId == entityInsertDTO.ProductId).Count() > 0))
                {
                    Errors.Add("Provide a valid product id");
                    return false;
                }

                return true;
            });

            return validHash && validProductsIDS;
        }
    }
}
