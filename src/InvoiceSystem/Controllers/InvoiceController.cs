using InvoiceSystem.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using InvoiceSystem.Services.Interfaces;
using InvoiceSystem.Api.DTO;

namespace InvoiceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper mapper;

        public InvoiceController(IMapper mapper, IInvoiceService invoiceService)
        {
            this.mapper = mapper;
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceRequestDto invoiceRequestDto)
        {
            try
            {
                var invoice = mapper.Map<Invoices>(invoiceRequestDto);
                var createdInvoice = await _invoiceService.CreateInvoice(invoice);
                return CreatedAtAction(nameof(GetInvoices), new { id = createdInvoice.Id.ToString() }, new { id = createdInvoice.Id.ToString() });
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _invoiceService.GetInvoices();
            if (invoices == null || !invoices.Any())
            {
                return NotFound("No invoices found.");
            }
            return Ok(mapper.Map<List<InvoiceResponseDto>>(invoices));
        }

        [HttpPost("{id}/payments")]
        public async Task<IActionResult> PayInvoice(int id, PaymentDto paymentDto)
        {
            try
            {
                await _invoiceService.ProcessPayment(id, paymentDto.Amount);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("process-overdue")]
        public async Task<IActionResult> ProcessOverdue(ProcessOverdue processOverdue)
        {
            try
            {
                await _invoiceService.ProcessOverdue(processOverdue.OverdueDays, processOverdue.LateFee);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
