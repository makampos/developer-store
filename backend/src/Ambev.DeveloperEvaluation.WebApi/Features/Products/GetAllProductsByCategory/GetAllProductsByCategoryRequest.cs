namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProductsByCategory;

public record GetAllProductsByCategoryRequest(
    string? Category = null,
    int? PageNumber = 1,
    int? PageSize = 10,
    string? Order = null)
{
    public static GetAllProductsByCategoryRequest Create(string category, int? pageNumber, int? pageSize, string?
        order = null)
    {
        return new GetAllProductsByCategoryRequest(category, pageNumber, pageSize, order);
    }

    public GetAllProductsByCategoryRequest IncludeCategory(string category)
    {
        return this with { Category = category };
    }
}