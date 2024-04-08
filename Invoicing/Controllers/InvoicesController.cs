using FluentValidation;
using Invoicing.DTOs;
using Invoicing.Services;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        IInvoiceService<InvoiceDTO, InvoiceInsertDTO> _invoiceService;
        IValidator<InvoiceInsertDTO> _invoiceInsertValidator;
        public InvoicesController([FromKeyedServices("InvoiceService")] IInvoiceService<InvoiceDTO, 
            InvoiceInsertDTO> invoiceService,
            IValidator<InvoiceInsertDTO> invoiceInsertValidator)
        {
            _invoiceService = invoiceService;
            _invoiceInsertValidator = invoiceInsertValidator;
        }

        [HttpGet]
        public async Task<IEnumerable<InvoiceDTO>> Get() => await _invoiceService.Get();

        [HttpGet("{hash}")]
        public async Task<ActionResult<InvoiceDTO>> GetByHash(string hash)
        {
            var invoiceDTO = await _invoiceService.GetByHash(hash);
            return invoiceDTO == null ? NotFound() : Ok(invoiceDTO);
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDTO>> Add(InvoiceInsertDTO invoiceInsertDTO)
        {
            var validationResult = await _invoiceInsertValidator.ValidateAsync(invoiceInsertDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_invoiceService.Validate(invoiceInsertDTO))
            {
                return BadRequest(_invoiceService.Errors);
            }

            var invoiceDTO = await _invoiceService.Add(invoiceInsertDTO);

            return CreatedAtAction(nameof(GetByHash), new { hash = invoiceDTO.Hash }, invoiceDTO);
        }
    }
}
