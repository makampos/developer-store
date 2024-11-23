namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

public record DeleteProductRequest(Guid Id)
{
    public static DeleteProductRequest Create(Guid id) => new(id);
}