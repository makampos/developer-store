namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;

public record GetAllProductsRequest(int? PageNumber = 1, int? PageSize = 10)
{
    public static GetAllProductsRequest Create(int pageNumber, int pageSize) =>
        new GetAllProductsRequest(pageNumber, pageSize);
}