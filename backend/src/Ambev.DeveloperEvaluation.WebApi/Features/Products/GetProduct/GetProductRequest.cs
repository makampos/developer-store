namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

public record GetProductRequest(Guid Id)
{
    public static GetProductRequest Create(Guid id) => new (id);
}