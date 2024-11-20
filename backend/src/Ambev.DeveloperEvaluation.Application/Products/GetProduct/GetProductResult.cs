using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public record GetProductResult(
    Guid Id,
    string Title,
    decimal Price,
    string Description,
    string Category,
    string Image,
    Rating Rating)
{
    public static GetProductResult Create(
        Guid id,
        string title,
        decimal price,
        string description,
        string category,
        string image,
        Rating rating)
    {
        return new GetProductResult(id, title, price, description, category, image, rating);
    }
}