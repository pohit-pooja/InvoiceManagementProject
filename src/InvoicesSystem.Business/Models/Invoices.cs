using InvoicesSystem.Business.Enums;

namespace InvoiceSystem.Models
{
    public class Invoices
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateOnly DueDate { get; set; }
        public int Status { get; set; } = (int)InvoiceStatus.Pending;
    }
}
