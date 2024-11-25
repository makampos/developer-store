using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public record UpdateSaleResponse(
    int Id,
    Guid UserId,
    DateTime SaleDate,
    List<SaleItem> SaleItems,
    decimal TotalSaleAmount,
    decimal TotalSaleDiscount,
    string Branch,
    bool IsCanceled)
{
    public UpdateSaleResponse() : this(
        0,
        Guid.Empty,
        DateTime.MinValue,
        [],
        0,
        0,
        string.Empty,
        false)
    {
    }
}