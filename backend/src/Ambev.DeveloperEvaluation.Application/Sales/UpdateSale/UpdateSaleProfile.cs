using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        CreateMap<Sale, UpdateSaleResult>()
            .ForPath(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForPath(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForPath(x => x.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForPath(x => x.SaleItems, opt => opt.MapFrom(src => src.SaleItems))
            .ForPath(x => x.TotalSaleAmount, opt => opt.MapFrom(src => src.TotalSaleAmount))
            .ForPath(x => x.TotalSaleDiscount, opt => opt.MapFrom(src => src.TotalSaleDiscount))
            .ForPath(x => x.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
            .ForPath(x => x.IsCanceled, opt => opt.MapFrom(src => src.IsCanceled));
    }
}