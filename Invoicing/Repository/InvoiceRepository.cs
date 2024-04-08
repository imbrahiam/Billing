using Invoicing.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Repository
{
    public class InvoiceRepository : IInvoiceRepository<Invoice>
    {
        InvoiceContext _invoiceContext;

        public InvoiceRepository(InvoiceContext invoiceContext)
        {
            _invoiceContext = invoiceContext;
        }

        public async Task<IEnumerable<Invoice>> Get() => await _invoiceContext.Invoices.ToListAsync();

        public async Task<Invoice> GetByHash(string hash) => await _invoiceContext.Invoices.FirstOrDefaultAsync(x => x.Hash == hash);
        public async Task Add(Invoice entity) => await _invoiceContext.Invoices.AddAsync(entity);

        public async Task Save()
        => await _invoiceContext.SaveChangesAsync();

        public IEnumerable<Invoice> Search(Func<Invoice, bool> filter) => _invoiceContext.Invoices.Where(filter).ToList();
        public IEnumerable<Client> SearchClient(Func<Client, bool> filter) => _invoiceContext.Clients.Where(filter).ToList();
    }
}
