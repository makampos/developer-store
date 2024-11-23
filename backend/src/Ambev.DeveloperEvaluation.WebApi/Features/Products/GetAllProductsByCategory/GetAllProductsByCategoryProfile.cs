using Ambev.DeveloperEvaluation.Application.Products.GetAllProductsByCategory;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProductsByCategory;

public class GetAllProductsByCategoryProfile : Profile
{
    public GetAllProductsByCategoryProfile()
    {
        CreateMap<GetAllProductsByCategoryRequest, GetAllProductsByCategoryCommand>();
    }
}