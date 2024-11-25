using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

public class GetAllSaleProfile : Profile
{
    public GetAllSaleProfile()
    {
        CreateMap<GetAllSaleResult, PagedResult<Sale>>()
            .ForPath(x => x.Items,
                o =>
                    o.MapFrom(s => s.Sales.Items))
            .ForPath(x => x.TotalCount,
                o =>
                    o.MapFrom(s => s.Sales.TotalCount))
            .ForPath(x => x.CurrentPage,
                o =>
                    o.MapFrom(s => s.Sales.CurrentPage))
            .ForPath(x => x.PageSize,
                o =>
                    o.MapFrom(s => s.Sales.PageSize))
            .ForPath(x => x.HasNextPage,
                o =>
                    o.MapFrom(s => s.Sales.HasNextPage))
            .ForPath(x => x.HasPreviousPage,
                o =>
                    o.MapFrom(s => s.Sales.HasPreviousPage))
            .ForPath(x => x.TotalPages,
                o =>
                    o.MapFrom(s => s.Sales.TotalPages)).ReverseMap();
    }
}