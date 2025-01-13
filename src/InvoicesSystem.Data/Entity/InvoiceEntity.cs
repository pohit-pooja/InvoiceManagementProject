namespace InvoicesSystem.Data.Entity
{
    public class InvoiceEntity
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateOnly DueDate { get; set; }
        public string Status { get; set; } = "pending";
    }
}
