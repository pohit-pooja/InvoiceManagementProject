using Microsoft.EntityFrameworkCore;
using InvoicesSystem.Data.Entity;

namespace InvoiceSystem.Data
{
    public class InvoiceDbContext : DbContext
    {
        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : base(options)
        {
        }

        public DbSet<InvoiceEntity> Invoice { get; set; }
    }
}
