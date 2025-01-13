using InvoicesSystem.Data.Entity;
using InvoicesSystem.Data.Repository.Interface;
using InvoiceSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoicesSystem.Data.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly InvoiceDbContext _context;
        public InvoiceRepository(InvoiceDbContext _context)
        {
            this._context = _context;
        }
        public async Task<InvoiceEntity> CreateInvoiceAsync(InvoiceEntity invoiceEntity)
        {
            await _context.Invoice.AddAsync(invoiceEntity);
            await _context.SaveChangesAsync();
            return invoiceEntity;
        }
        public async Task<List<InvoiceEntity>> GetInvoiceAsync()
        {
            var invoiceEntity = await _context.Invoice.ToListAsync();
            return invoiceEntity;
        }
        public async Task<InvoiceEntity> GetInvoiceByIdAsync(int id)
        {
            var invoiceEntity = _context.Invoice.FirstOrDefault(i => i.Id == id);
            return invoiceEntity;
        }
        public async Task ProcessPaymentAsync(InvoiceEntity invoiceEntity)
        {
            _context.Invoice.Update(invoiceEntity);
            await _context.SaveChangesAsync();
        }        
        public async Task<List<InvoiceEntity>> GetOverdueInvoicesAsync(int overdueDays, decimal lateFee)
        {
            var now = DateOnly.FromDateTime(DateTime.UtcNow);
            var invoiceEntity = await _context.Invoice
            .Where(i => i.Status == "pending" && i.DueDate < now)
            .ToListAsync();
            return invoiceEntity;
        }
        public async Task CreateAndProcessPaymentAsync(InvoiceEntity newInvoiceEntity, InvoiceEntity invoice)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Invoice.Update(invoice);
                await _context.SaveChangesAsync();

                await _context.Invoice.AddAsync(newInvoiceEntity);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }           
        }
    }
}
