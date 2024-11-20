namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public record DeleteProductResponse(bool Success)
{
    public static DeleteProductResponse Create(bool success) => new(success);
}