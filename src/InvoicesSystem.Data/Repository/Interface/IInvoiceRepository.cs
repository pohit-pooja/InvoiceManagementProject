using InvoicesSystem.Data.Entity;

namespace InvoicesSystem.Data.Repository.Interface
{
    public interface IInvoiceRepository
    {
        Task<InvoiceEntity> CreateInvoiceAsync(InvoiceEntity invoiceEntity);
        Task<List<InvoiceEntity>> GetInvoiceAsync();
        Task<InvoiceEntity> GetInvoiceByIdAsync(int id);
        Task ProcessPaymentAsync(InvoiceEntity invoiceEntity);
        Task<List<InvoiceEntity>> GetOverdueInvoicesAsync(int overdueDays, decimal lateFee);
        Task CreateAndProcessPaymentAsync(InvoiceEntity newInvoiceEntity, InvoiceEntity invoice);
    }
}
