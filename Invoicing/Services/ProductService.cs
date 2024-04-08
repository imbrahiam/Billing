using AutoMapper;
using Invoicing.DTOs;
using Invoicing.Models;
using Invoicing.Repository;

namespace Invoicing.Services
{
    public class ProductService : ICommonService<ProductDTO, ProductInsertDTO, ProductUpdateDTO>
    {
        private IRepository<Product> _productRepository;
        private IMapper _mapper;

        public ProductService(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }

        public List<string> Errors { get; }
        public async Task<IEnumerable<ProductDTO>> Get()
        {
            var products = await _productRepository.Get();

            return products.Select(p => _mapper.Map<ProductDTO>(p));
        }

        public async Task<ProductDTO> GetByName(string name)
        {
            var product = await _productRepository.GetByName(name);

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> GetById(int id)
        {
            var product = await _productRepository.GetById(id);

            if (product != null)
            {
                var productDTO = _mapper.Map<ProductDTO>(product);

                return productDTO;
            }

            return null!;
        }

        public async Task<ProductDTO> Add(ProductInsertDTO entityInsertDTO)
        {
            var product = _mapper.Map<Product>(entityInsertDTO);

            await _productRepository.Add(product);
            await _productRepository.Save();

            var productDTO = _mapper.Map<ProductDTO>(product);
            return productDTO;
        }

        public async Task<ProductDTO> Update(int id, ProductUpdateDTO entityUpdateDTO)
        {
            var product = await _productRepository.GetById(id);

            if (product != null)
            {
                product = _mapper.Map<ProductUpdateDTO, Product>(entityUpdateDTO, product);
                _productRepository.Update(product);
                await _productRepository.Save();

                var productDTO = _mapper.Map<ProductDTO>(product);
                return productDTO;
            }

            return null!;
        }

        public async Task<ProductDTO> Delete(int id)
        {
            var product = await _productRepository.GetById(id);

            if (product != null)
            {
                var productDTO = _mapper.Map<ProductDTO>(product);

                _productRepository.Delete(product);
                await _productRepository.Save();

                return productDTO;
            }

            return null!;
        }

        public bool Validate(ProductInsertDTO entityInsertDTO)
        {
            if (_productRepository.Search(p => p.Name == entityInsertDTO.Name).Count() > 0)
            {
                Errors.Add("Repeated product name");
                return false;
            }

            return true;
        }

        public bool Validate(ProductUpdateDTO entityUpdateDTO)
        {
            if (_productRepository.Search(p => p.Name == entityUpdateDTO.Name && p.ProductId != entityUpdateDTO.Id).Count() > 0)
            {
                Errors.Add("Product name already taken");

                return false;
            }

            return true;
        }
    }
}
