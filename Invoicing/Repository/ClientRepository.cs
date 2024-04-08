using Invoicing.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Repository
{
    public class ClientRepository : IClientRepository<Client>
    {
        InvoiceContext _invoiceContext;
        public ClientRepository(InvoiceContext invoiceContext)
        {
            _invoiceContext = invoiceContext;
        }

        public async Task<IEnumerable<Client>> Get() => await _invoiceContext.Clients.ToListAsync();

        //public async Task<Client> GetById(int id) => await _invoiceContext.Clients.FindAsync(id);
        public async Task<Client> GetByMat(string mat) => await _invoiceContext.Clients.FirstOrDefaultAsync(x => x.Mat == mat);

        public async Task Add(Client entity) => await _invoiceContext.Clients.AddAsync(entity);
        public void Update(Client entity)
        {
            _invoiceContext.Attach(entity);
            _invoiceContext.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(Client entity)
            => _invoiceContext.Clients.Remove(entity);

        public async Task Save()
        => await _invoiceContext.SaveChangesAsync();

        public IEnumerable<Client> Search(Func<Client, bool> filter) => _invoiceContext.Clients.Where(filter).ToList();
    }
}
