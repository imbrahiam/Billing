using Invoicing.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Repository
{
    public class ItemRepository : IItemRepository<Item>
    {
        InvoiceContext _invoiceContext;
        public ItemRepository(InvoiceContext invoiceContext)
        {
            _invoiceContext = invoiceContext;
        }

        public async Task<IEnumerable<Item>> GetAllByHash(string hash) => await _invoiceContext.Items.Where(i => i.Hash == hash).ToListAsync();

        public async Task Add(IEnumerable<Item> items) => await _invoiceContext.Items.AddRangeAsync(items);

        public async Task Save() => await _invoiceContext.SaveChangesAsync();

        public IEnumerable<Item> Search(Func<Item, bool> filter) => _invoiceContext.Items.Where(filter).ToList();
        public IEnumerable<Invoice> SearchInvoice(Func<Invoice, bool> filter) => _invoiceContext.Invoices.Where(filter).ToList();

        public IEnumerable<Product> SearchProduct(Func<Product, bool> filter) => _invoiceContext.Products.Where(filter).ToList();
    }
}
