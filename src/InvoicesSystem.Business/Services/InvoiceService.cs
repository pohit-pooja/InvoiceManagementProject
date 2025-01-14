using AutoMapper;
using InvoicesSystem.Business.Enums;
using InvoicesSystem.Data.Entity;
using InvoicesSystem.Data.Repository.Interface;
using InvoiceSystem.Models;
using InvoiceSystem.Services.Interfaces;

namespace InvoiceSystem.Services
{
    public class InvoiceService : IInvoiceService 
    {
        private readonly IMapper mapper;
        private readonly IInvoiceRepository invoiceRepository;

        public InvoiceService(IMapper mapper, IInvoiceRepository invoiceRepository)
        {
            this.mapper = mapper;
            this.invoiceRepository = invoiceRepository;
        }

        public async Task<Invoices> CreateInvoice(Invoices invoice)
        {
            if (invoice.DueDate < DateOnly.FromDateTime(DateTime.UtcNow.Date))
                throw new ArgumentException("Due date cannot be in the past.");
            if (invoice.Amount <= 0)
                throw new ArgumentException("Amount must be greater than 0.");

            var invoiceEntity = mapper.Map<InvoiceEntity>(invoice);
            var invoices = await invoiceRepository.CreateInvoiceAsync(invoiceEntity);
            return mapper.Map<Invoices>(invoices);
        }

        public async Task<List<Invoices>> GetInvoices()
        {
            var invoices = await invoiceRepository.GetInvoiceAsync();
            return mapper.Map<List<Invoices>>(invoices);
        }

        public async Task ProcessPayment(int id, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Payment amount must be greater than 0.",nameof(amount));

            var invoiceEntity = await invoiceRepository.GetInvoiceByIdAsync(id);

            if (invoiceEntity == null || invoiceEntity.Status != (int)InvoiceStatus.Pending)
                throw new Exception("Invoice not found or not payable.");
            if (amount > invoiceEntity.Amount)
                throw new ArgumentException("Payment amount exceeds the outstanding invoice amount.", nameof(amount));

            invoiceEntity.PaidAmount += amount;
            invoiceEntity.Amount -= amount;

            if (invoiceEntity.Amount == 0)
            {
                invoiceEntity.Status = (int)InvoiceStatus.Paid;
            }
            await invoiceRepository.ProcessPaymentAsync(invoiceEntity);
        }

        public async Task ProcessOverdue(int overdueDays, decimal lateFee)
        {
            if (overdueDays <= 0)
                throw new ArgumentException("overdueDays must be greater than 0.", nameof(overdueDays));
            if (lateFee < 0)
                throw new ArgumentException("lateFee can not be less than 0.", nameof(lateFee));

            var overdueInvoices = await invoiceRepository.GetOverdueInvoicesAsync(overdueDays);
            
            foreach (var invoice in overdueInvoices)
            {
                if (invoice.PaidAmount > 0)
                {
                    invoice.Status = (int)InvoiceStatus.Paid;
                }
                else
                {
                    invoice.Status = (int)InvoiceStatus.Void;
                }

                var newInvoice = new Invoices
                {
                    Amount = invoice.Amount + lateFee,
                    DueDate = invoice.DueDate.AddDays(overdueDays),
                };
                var newInvoiceEntity = mapper.Map<InvoiceEntity>(newInvoice);
                await invoiceRepository.CreateAndProcessPaymentAsync(newInvoiceEntity, invoice);
            }            
        }
    }
}
