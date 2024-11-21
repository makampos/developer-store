using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

public class GetAllProductsProfile : Profile
{
    public GetAllProductsProfile()
    {
        CreateMap<PagedResult<Product>, GetAllProductsResult>()
            .ForPath(x => x.Products, opt =>
                opt.MapFrom(src => src))
            .ForPath(x => x.Products.CurrentPage, opt =>
                opt.MapFrom(src => src.CurrentPage))
            .ForPath(x => x.Products.PageSize, opt =>
                opt.MapFrom(src => src.PageSize))
            .ForPath(x => x.Products.TotalCount, opt =>
                opt.MapFrom(src => src.TotalCount))
            .ForPath(x => x.Products.TotalPages, opt =>
                opt.MapFrom(src => src.TotalPages))
            .ForPath(x => x.Products.HasPreviousPage, opt =>
                opt.MapFrom(src => src.HasPreviousPage))
            .ForPath(x => x.Products.HasNextPage, opt =>
                opt.MapFrom(src => src.HasNextPage));
    }
}