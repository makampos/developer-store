using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Carts.DeleteCart;

public class DeleteCartProfile : Profile
{
    public DeleteCartProfile()
    {
        CreateMap<DeleteCartRequest, DeleteCartCommand>();
        CreateMap<DeleteCartResult, DeleteCartResponse>();
    }
}