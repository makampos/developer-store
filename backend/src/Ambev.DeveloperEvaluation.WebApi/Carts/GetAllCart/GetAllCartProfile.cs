using Ambev.DeveloperEvaluation.Application.Carts.GetAllCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Carts.GetAllCart;

public class GetAllCartProfile : Profile
{
    public GetAllCartProfile()
    {
        CreateMap<GetAllCartRequest,  GetAllCardCommand>();
    }
}