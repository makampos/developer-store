using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

public record GetProductResponse(
    Guid Id,
    string Title,
    decimal Price,
    string Description,
    string Category,
    string Image,
    Rating Rating);