namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;

public record GetAllProductsRequest(int? PageNumber = 1, int? PageSize = 10, string? Order = null)
{
    public static GetAllProductsRequest Create(int pageNumber, int pageSize, string? order = null) =>
        new GetAllProductsRequest(pageNumber, pageSize, order);
}