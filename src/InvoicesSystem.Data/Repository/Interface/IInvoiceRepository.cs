using InvoicesSystem.Data.Entity;

namespace InvoicesSystem.Data.Repository.Interface
{
    public interface IInvoiceRepository
    {
        /** <summary>
         * Creates a new invoice in the database.
         * </summary>
         * <param name="invoiceEntity">The invoice entity containing the details to be created.</param>
         * <returns>Id of the created invoice entity.</returns>
         */
        Task<InvoiceEntity> CreateInvoiceAsync(InvoiceEntity invoiceEntity);

        /** <summary>
         * Retrieves the list of all invoices from the database.
         * </summary>
         * <returns>A list of all invoice entities.</returns>
         */
        Task<List<InvoiceEntity>> GetInvoiceAsync();

        /** <summary>
         * Retrieves a specific invoice by its ID.
         * </summary>
         * <param name="id">The ID of the invoice to retrieve.</param>
         * <returns>The invoice entity matching the given ID.</returns>
         */
        Task<InvoiceEntity> GetInvoiceByIdAsync(int id);

        /** <summary>
         * Processes a payment for the specified invoice and updates its payment details in the database.
         * </summary>
         * <param name="invoiceEntity">The invoice entity containing updated payment details.</param>
         * <returns>Returns a Task that represents the asynchronous operation.</returns>
         */
        Task ProcessPaymentAsync(InvoiceEntity invoiceEntity);

        /** <summary>
         * Retrieves invoices that are overdue.
         * </summary>
         * <param name="overdueDays">The number of days to add to the previous due date to calculate the new due date.</param>
         * <returns>A list of overdue invoice entities.</returns>
         */
        Task<List<InvoiceEntity>> GetOverdueInvoicesAsync(int overdueDays);

        /** <summary>
         * Creates a new invoice and processes a payment for an existing invoice in a single transaction.
         * </summary>
         * <param name="newInvoiceEntity">The new invoice entity to be created.</param>
         * <param name="invoice">The existing invoice entity to process the payment for.</param>
         * <returns>Returns a Task that represents the asynchronous operation.</returns>
         */
        Task CreateAndProcessPaymentAsync(InvoiceEntity newInvoiceEntity, InvoiceEntity invoice);
    }
}
