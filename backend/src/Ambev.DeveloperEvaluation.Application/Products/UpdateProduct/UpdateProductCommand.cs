using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Title,
    decimal Price,
    string Description,
    string Category,
    string Image,
    Rating Rating) : IRequest<UpdateProductResult>
{
    public UpdateProductCommand() : this(
        Guid.Empty,
        string.Empty,
        0m,
        string.Empty,
        string.Empty,
        string.Empty,
        Rating.Create(0, 0))
    { }
}