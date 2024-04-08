using FluentValidation;
using Invoicing.DTOs;
using Invoicing.Services;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ICommonService<ProductDTO, ProductInsertDTO, ProductUpdateDTO> _productService;
        IValidator<ProductInsertDTO> _productInsertValidator;
        IValidator<ProductUpdateDTO> _productUpdateValidator;

        public ProductsController([FromKeyedServices("ProductService")] ICommonService<ProductDTO, ProductInsertDTO, ProductUpdateDTO> productService,
            IValidator<ProductInsertDTO> productInsertValidator,
            IValidator<ProductUpdateDTO> productUpdateValidator)
        {
            _productService = productService;
            _productInsertValidator = productInsertValidator;
            _productUpdateValidator = productUpdateValidator;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> Get() => await _productService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var productDTO = await _productService.GetById(id);
            return productDTO == null ? NotFound() : Ok(productDTO);
        }

        [HttpGet("search/{term}")]
        public async Task<ProductDTO> GetByName(string term) => await _productService.GetByName(term);

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Add(ProductInsertDTO productInsertDTO)
        {
            var validationResult = await _productInsertValidator.ValidateAsync(productInsertDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_productService.Validate(productInsertDTO))
            {
                return BadRequest(_productService.Errors);
            }

            var productDTO = await _productService.Add(productInsertDTO);

            return CreatedAtAction(nameof(GetById), new { id = productDTO.Id }, productDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> Update(int id, ProductUpdateDTO productUpdateDTO)
        {
            var validationResult = await _productUpdateValidator.ValidateAsync(productUpdateDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }


            if (!_productService.Validate(productUpdateDTO))
            {
                return BadRequest(_productService.Errors);
            }

            var productDTO = await _productService.Update(id, productUpdateDTO);

            return productDTO == null ? NotFound() : Ok(productDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var productDTO = await _productService.Delete(id);

            return productDTO == null ? NotFound() : Ok(productDTO);
        }
    }
}
