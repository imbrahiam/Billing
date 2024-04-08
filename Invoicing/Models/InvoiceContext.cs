using Microsoft.EntityFrameworkCore;

namespace Invoicing.Models
{
    public class InvoiceContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }

        public InvoiceContext(DbContextOptions<InvoiceContext> options) : base(options) { }
    }
}
