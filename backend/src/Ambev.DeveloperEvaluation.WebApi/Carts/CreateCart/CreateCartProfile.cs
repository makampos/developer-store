using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Carts.CreateCart;

public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        CreateMap<CreateCartRequest, CreateCartCommand>();
        CreateMap<CreateCartResult, CreateCartResponse>();
    }
}