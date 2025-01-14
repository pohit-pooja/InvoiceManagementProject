using AutoMapper;
using InvoiceSystem.Models;
using InvoicesSystem.Data.Entity;
using InvoiceSystem.Api.DTO;
using InvoicesSystem.Business.Enums;

namespace InvoiceSystem.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<InvoiceRequestDto, Invoices>().ReverseMap();
            CreateMap<InvoiceEntity, Invoices>().ReverseMap();
            CreateMap<Invoices, InvoiceResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((InvoiceStatus)src.Status).ToString()))
            .ReverseMap()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)Enum.Parse<InvoiceStatus>(src.Status)));
        }
    }
}
