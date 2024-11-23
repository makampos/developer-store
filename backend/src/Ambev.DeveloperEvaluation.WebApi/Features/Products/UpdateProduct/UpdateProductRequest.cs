using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public record UpdateProductRequest(
    [property: JsonIgnore] Guid Id,
    string Title,
    decimal Price,
    string Description,
    string Category,
    string Image,
    Rating Rating)
{

    // Necessary for Faker
    public UpdateProductRequest() : this(
        Guid.Empty,
        string.Empty,
        0,
        string.Empty,
        string.Empty,
        string.Empty,
        Rating.Create(0, 0))
    {
    }

    public UpdateProductRequest IncludeId(Guid id)
    {
        return this with { Id = id };
    }
}