using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        CreateMap<CreateCartCommand, Cart>()
            .ForPath(x => x.UserId, x => x.MapFrom(y => y.UserId))
            .ForPath(x => x.Date, x => x.MapFrom(y => y.Date))
            .ForPath(x => x.Products, x => x.MapFrom(y => y.CartItems));

        CreateMap<Cart, CreateCartResult>()
        .ForPath(x => x.UserId, x => x.MapFrom(y => y.UserId))
        .ForPath(x => x.Date, x => x.MapFrom(y => y.Date))
        .ForPath(x => x.CartItems, x => x.MapFrom(y => y.Products));

    }
}