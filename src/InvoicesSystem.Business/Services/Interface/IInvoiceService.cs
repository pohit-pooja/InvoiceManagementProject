using InvoiceSystem.Models;

namespace InvoiceSystem.Services.Interfaces
{
    public interface IInvoiceService
    {
        /** <summary>
         * Creates a new invoice and returns the created invoice.
         * </summary>
         * <param name="invoice">The invoice details to be created.</param>
         * <returns>The created invoice object.</returns>
         */
        Task<Invoices> CreateInvoice(Invoices invoice);

        /** <summary>
         * Retrieves the list of all invoices.
         * </summary>
         * <returns>A list of all invoices.</returns> 
         */
        Task<List<Invoices>> GetInvoices();

        /** <summary>
         * Processes a payment for a specific invoice and updates the payment details.
         * </summary>
         * <param name="id">The ID of the invoice to process payment for.</param>
         * <param name="amount">The amount being paid for the invoice.</param>
         * <returns>Returns a Task that represents the asynchronous operation.</returns>
         */
        Task ProcessPayment(int id, decimal amount);

        /** <summary>
         * Processes overdue invoices based on the duedate and applies the late fee, overdue days .
         * </summary>
         * <param name="overdueDays">The number of days to add to the previous due date to calculate the new due date.</param>
         * <param name="lateFee">The late fee to be applied to overdue invoices.</param>
         * <returns>Returns a Task that represents the asynchronous operation.</returns>
         */
        Task ProcessOverdue(int overdueDays, decimal lateFee);
    }
}
