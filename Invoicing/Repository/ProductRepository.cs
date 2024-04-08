using Invoicing.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        InvoiceContext _invoiceContext;

        public ProductRepository(InvoiceContext invoiceContext)
        {
            _invoiceContext = invoiceContext;
        }

        public async Task<IEnumerable<Product>> Get() => await _invoiceContext.Products.ToListAsync();
        public async Task<Product> GetByName(string name)
        {
            var result = await _invoiceContext.Products.FirstOrDefaultAsync(p => p.Name.ToLower().Contains(name.ToLower()));
            return result != null ? result : null!;
        }

        public async Task<Product> GetById(int id)
        {
            var result = await _invoiceContext.Products.FindAsync(id);
            return result != null ? result : null!;
        }
        public async Task Add(Product entity) => await _invoiceContext.Products.AddAsync(entity);
        public void Update(Product entity)
        {
            _invoiceContext.Attach(entity);
            _invoiceContext.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(Product entity)
            => _invoiceContext.Products.Remove(entity);

        public async Task Save()
        => await _invoiceContext.SaveChangesAsync();

        public IEnumerable<Product> Search(Func<Product, bool> filter) => _invoiceContext.Products.Where(filter).ToList();
    }
}
