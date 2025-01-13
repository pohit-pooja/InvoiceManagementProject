using AutoMapper;
using InvoiceSystem.Models;
using InvoicesSystem.Data.Entity;
using InvoiceSystem.Api.DTO;

namespace InvoiceSystem.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<InvoiceRequestDto, Invoices>().ReverseMap();
            CreateMap<InvoiceEntity, Invoices>().ReverseMap();
            CreateMap<InvoiceResponseDto, Invoices>().ReverseMap();
        }
    }
}
