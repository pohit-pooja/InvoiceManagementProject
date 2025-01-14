using AutoMapper;
using InvoiceSystem.Models;
using InvoiceSystem.Services;
using InvoiceSystem.Services.Interfaces;
using Moq;
using InvoicesSystem.Data.Repository.Interface;
using InvoicesSystem.Data.Entity;
using InvoicesSystem.Business.Enums;

namespace InvoicesSystem.Test
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IInvoiceRepository> _mockInvoiceRepository;
        private readonly IInvoiceService _invoiceService;

        public InvoiceServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockInvoiceRepository = new Mock<IInvoiceRepository>();
            _invoiceService = new InvoiceService(_mockMapper.Object, _mockInvoiceRepository.Object);
        }

        [Fact]
        public async Task CreateInvoice_ShouldThrowException_WhenDueDateIsInThePast()
        {
            var invoice = new Invoices { DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)), Amount = 100 };

            await Assert.ThrowsAsync<ArgumentException>(() => _invoiceService.CreateInvoice(invoice));
        }

        [Fact]
        public async Task CreateInvoice_ShouldThrowException_WhenAmountIsZeroOrLess()
        {
            var invoice = new Invoices { DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Amount = 0 };

            await Assert.ThrowsAsync<ArgumentException>(() => _invoiceService.CreateInvoice(invoice));
        }

        [Fact]
        public async Task CreateInvoice_ShouldCreateInvoice_WhenValidInput()
        {
            var invoice = new Invoices { DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), Amount = 100 };
            var invoiceEntity = new InvoiceEntity();

            _mockMapper.Setup(m => m.Map<InvoiceEntity>(invoice)).Returns(invoiceEntity);
            _mockInvoiceRepository.Setup(r => r.CreateInvoiceAsync(invoiceEntity)).ReturnsAsync(invoiceEntity);
            _mockMapper.Setup(m => m.Map<Invoices>(invoiceEntity)).Returns(invoice);

            var result = await _invoiceService.CreateInvoice(invoice);

            Assert.Equal(invoice, result);
        }

        [Fact]
        public async Task GetInvoices_ShouldReturnListOfInvoices()
        {
            var invoiceEntities = new List<InvoiceEntity> { new InvoiceEntity() };
            var invoices = new List<Invoices> { new Invoices() };

            _mockInvoiceRepository.Setup(r => r.GetInvoiceAsync()).ReturnsAsync(invoiceEntities);
            _mockMapper.Setup(m => m.Map<List<Invoices>>(invoiceEntities)).Returns(invoices);

            var result = await _invoiceService.GetInvoices();

            Assert.Equal(invoices, result);
        }

        [Fact]
        public async Task ProcessPayment_ShouldThrowException_WhenAmountIsZeroOrLess()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _invoiceService.ProcessPayment(1, 0));
        }

        [Fact]
        public async Task ProcessPayment_ShouldThrowException_WhenInvoiceNotFoundOrNotPayable()
        {
            _mockInvoiceRepository.Setup(r => r.GetInvoiceByIdAsync(It.IsAny<int>())).ReturnsAsync((InvoiceEntity)null);

            await Assert.ThrowsAsync<Exception>(() => _invoiceService.ProcessPayment(1, 100));
        }

        [Fact]
        public async Task ProcessPayment_ShouldThrowException_WhenPaymentExceedsOutstandingAmount()
        {
            var invoice = new InvoiceEntity { Amount = 50, Status = (int)InvoiceStatus.Pending };
            _mockInvoiceRepository.Setup(r => r.GetInvoiceByIdAsync(It.IsAny<int>())).ReturnsAsync(invoice);

            await Assert.ThrowsAsync<ArgumentException>(() => _invoiceService.ProcessPayment(1, 100));
        }

        [Fact]
        public async Task ProcessPayment_ShouldUpdateInvoice_WhenValidPayment()
        {
            var invoice = new InvoiceEntity { Id = 1, Amount = 100, PaidAmount = 0, Status = (int)InvoiceStatus.Pending };

            _mockInvoiceRepository.Setup(r => r.GetInvoiceByIdAsync(invoice.Id)).ReturnsAsync(invoice);
            _mockInvoiceRepository.Setup(r => r.ProcessPaymentAsync(invoice)).Returns(Task.CompletedTask);

            await _invoiceService.ProcessPayment(invoice.Id, 50);

            Assert.Equal(50, invoice.PaidAmount);
            Assert.Equal(50, invoice.Amount);
            _mockInvoiceRepository.Verify(r => r.ProcessPaymentAsync(invoice), Times.Once);
        }

        [Fact]
        public async Task ProcessOverdue_ShouldThrowException_WhenOverdueDaysOrLateFeeInvalid()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _invoiceService.ProcessOverdue(0, 10));
            await Assert.ThrowsAsync<ArgumentException>(() => _invoiceService.ProcessOverdue(10, -1));
        }

        [Fact]
        public async Task ProcessOverdue_ShouldUpdateInvoicesAndCreateNew_OnOverdueInvoices()
        {
            var overdueInvoices = new List<InvoiceEntity>
            {
                new InvoiceEntity { Id = 1, Amount = 100, PaidAmount = 0, Status = (int)InvoiceStatus.Pending, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)) },
                new InvoiceEntity { Id = 2, Amount = 200, PaidAmount = 0, Status = (int)InvoiceStatus.Pending, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) },
                new InvoiceEntity { Id = 3, Amount = 300, PaidAmount = 10, Status = (int)InvoiceStatus.Pending, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)) },
                new InvoiceEntity { Id = 4, Amount = 300, PaidAmount = 100, Status = (int)InvoiceStatus.Pending, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) },
            };

            _mockInvoiceRepository.Setup(r => r.GetOverdueInvoicesAsync(It.IsAny<int>())).ReturnsAsync(overdueInvoices);
            _mockInvoiceRepository.Setup(r => r.CreateAndProcessPaymentAsync(It.IsAny<InvoiceEntity>(), It.IsAny<InvoiceEntity>())).Returns(Task.CompletedTask);

            await _invoiceService.ProcessOverdue(10, 20);

            foreach (var invoice in overdueInvoices)
            {
                if(invoice.PaidAmount == 0)
                    Assert.Equal((int)InvoiceStatus.Void, invoice.Status);
                else
                    Assert.Equal((int)InvoiceStatus.Paid, invoice.Status);
            }
            _mockInvoiceRepository.Verify(r => r.CreateAndProcessPaymentAsync(It.IsAny<InvoiceEntity>(), It.IsAny<InvoiceEntity>()), Times.Exactly(overdueInvoices.Count));
        }
    }
}