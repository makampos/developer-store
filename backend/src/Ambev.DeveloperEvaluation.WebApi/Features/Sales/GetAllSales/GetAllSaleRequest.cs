namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;

public record GetAllSaleRequest(int? PageNumber = 1, int? PageSize = 10, string? Order = null)
{
    public static GetAllSaleRequest Create(int pageNumber, int pageSize, string? order = null) =>
        new GetAllSaleRequest(pageNumber, pageSize, order);
}