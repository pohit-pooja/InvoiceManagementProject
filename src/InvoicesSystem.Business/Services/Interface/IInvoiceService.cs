using InvoiceSystem.Models;

namespace InvoiceSystem.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<Invoices> CreateInvoice(Invoices invoice);
        Task<List<Invoices>> GetInvoices();
        Task ProcessPayment(int id, decimal amount);
        Task ProcessOverdue(int overdueDays, decimal lateFee);
    }
}
