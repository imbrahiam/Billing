using Invoicing.Models;

namespace Invoicing.Repository
{
    public interface IInvoiceRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> Get();
        Task<TEntity> GetByHash(string hash);
        Task Add(TEntity entity);
        Task Save();
        IEnumerable<TEntity> Search(Func<TEntity, bool> filter);
        IEnumerable<Client> SearchClient(Func<Client, bool> filter);
    }
}
