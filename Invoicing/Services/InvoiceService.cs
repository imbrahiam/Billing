using AutoMapper;
using Invoicing.DTOs;
using Invoicing.Models;
using Invoicing.Repository;

namespace Invoicing.Services
{
    public class InvoiceService : IInvoiceService<InvoiceDTO, InvoiceInsertDTO>
    {
        IInvoiceRepository<Invoice> _invoiceRepository;
        IMapper _mapper;

        public InvoiceService(IInvoiceRepository<Invoice> invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }

        public List<string> Errors { get; }
        public async Task<IEnumerable<InvoiceDTO>> Get()
        {
            var invoices = await _invoiceRepository.Get();

            return invoices.Select(p => _mapper.Map<InvoiceDTO>(p));
        }

        public async Task<InvoiceDTO> GetByHash(string hash)
        {
            var invoice = await _invoiceRepository.GetByHash(hash);

            if (invoice != null)
            {
                var invoiceDTO = _mapper.Map<InvoiceDTO>(invoice);

                return invoiceDTO;
            }

            return null!;
        }

        public async Task<InvoiceDTO> Add(InvoiceInsertDTO entityInsertDTO)
        {
            var invoice = _mapper.Map<Invoice>(entityInsertDTO);

            invoice.date = DateTime.Now;

            await _invoiceRepository.Add(invoice);
            await _invoiceRepository.Save();

            var invoiceDTO = _mapper.Map<InvoiceDTO>(invoice);
            return invoiceDTO;
        }

        public bool Validate(InvoiceInsertDTO entityInsertDTO)
        {
            if (!(_invoiceRepository.SearchClient(c => c.ClientId == entityInsertDTO.ClientId).Count() > 0))
            {
                Errors.Add("Provide a valid client id");
                return false;
            }
            else if (_invoiceRepository.Search(p => p.Hash == entityInsertDTO.Hash).Count() > 0)
            {
                Errors.Add("Hash must be unique");
                return false;
            }

            return true;
        }
    }
}
