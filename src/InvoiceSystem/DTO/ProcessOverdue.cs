namespace InvoiceSystem.Api.DTO
{
    public class ProcessOverdue
    {
        public int OverdueDays { get; set; }
        public decimal LateFee { get; set; }
    }
}
