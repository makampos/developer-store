using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public record UpdateProductResult(
    Guid Id,
    string Title,
    decimal Price,
    string Description,
    string Category,
    string Image,
    Rating Rating)
{
    public static UpdateProductResult Create(
        Guid id,
        string title,
        decimal price,
        string description,
        string category,
        string image,
        Rating rating)
    {
        return new UpdateProductResult(id, title, price, description, category, image, rating);
    }
}