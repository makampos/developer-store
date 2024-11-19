using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class CreateProductRequest : Profile
{
    public CreateProductRequest()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>();
    }
}