using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

public class GetAllProductsProfile : Profile
{
    public GetAllProductsProfile()
    {
        CreateMap<GetAllProductsResult, PagedResult<Product>>()
            .ForPath(x => x.Items,
                o =>
                    o.MapFrom(s => s.Products.Items))
            .ForPath(x => x.TotalCount,
                o =>
                    o.MapFrom(s => s.Products.TotalCount))
            .ForPath(x => x.CurrentPage,
                o =>
                    o.MapFrom(s => s.Products.CurrentPage))
            .ForPath(x => x.PageSize,
                o =>
                    o.MapFrom(s => s.Products.PageSize))
            .ForPath(x => x.HasNextPage,
                o =>
                    o.MapFrom(s => s.Products.HasNextPage))
            .ForPath(x => x.HasPreviousPage,
                o =>
                    o.MapFrom(s => s.Products.HasPreviousPage))
            .ForPath(x => x.TotalPages,
                o =>
                    o.MapFrom(s => s.Products.TotalPages)).ReverseMap();
    }
}