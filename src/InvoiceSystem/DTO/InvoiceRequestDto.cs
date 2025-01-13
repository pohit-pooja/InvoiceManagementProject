using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceSystem.Api.DTO
{
    public class InvoiceRequestDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        [JsonPropertyName("dueDate")]
        [DataType(DataType.Date)]
        public DateOnly DueDate { get; set; }
    }
}
