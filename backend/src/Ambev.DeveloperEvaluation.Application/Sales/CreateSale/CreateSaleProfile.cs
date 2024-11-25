using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<Sale, CreateSaleResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.TotalSaleAmount, opt => opt.MapFrom(src => src.TotalSaleAmount))
            .ForMember(dest => dest.TotalSaleDiscount, opt => opt.MapFrom(src => src.TotalSaleDiscount))
            .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.SaleItems))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
            .ForMember(dest => dest.IsCanceled, opt => opt.MapFrom(src => src.IsCanceled));
    }
}