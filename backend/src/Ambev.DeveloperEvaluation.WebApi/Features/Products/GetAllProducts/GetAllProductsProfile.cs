using Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;

public class GetAllProductsProfile : Profile
{
    public GetAllProductsProfile()
    {
        CreateMap<GetAllProductsRequest, GetAllProductsCommand>();
    }
}