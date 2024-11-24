using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetAllCart;

public class GetAllCartProfile : Profile
{
    public GetAllCartProfile()
    {
        CreateMap<GetAllCartResult, PagedResult<Cart>>()
            .ForPath(x => x.Items,
                o =>
                    o.MapFrom(s => s.Carts.Items))
            .ForPath(x => x.TotalCount,
                o =>
                    o.MapFrom(s => s.Carts.TotalCount))
            .ForPath(x => x.CurrentPage,
                o =>
                    o.MapFrom(s => s.Carts.CurrentPage))
            .ForPath(x => x.PageSize,
                o =>
                    o.MapFrom(s => s.Carts.PageSize))
            .ForPath(x => x.HasNextPage,
                o =>
                    o.MapFrom(s => s.Carts.HasNextPage))
            .ForPath(x => x.HasPreviousPage,
                o =>
                    o.MapFrom(s => s.Carts.HasPreviousPage))
            .ForPath(x => x.TotalPages,
                o =>
                    o.MapFrom(s => s.Carts.TotalPages)).ReverseMap();
    }
}