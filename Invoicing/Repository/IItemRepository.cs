using Invoicing.Models;

namespace Invoicing.Repository
{
    public interface IItemRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllByHash(string hash);
        Task Add(IEnumerable<TEntity> items);
        Task Save();
        IEnumerable<TEntity> Search(Func<TEntity, bool> filter);
        IEnumerable<Invoice> SearchInvoice(Func<Invoice, bool> filter);

        IEnumerable<Product> SearchProduct(Func<Product, bool> filter);
    }
}
