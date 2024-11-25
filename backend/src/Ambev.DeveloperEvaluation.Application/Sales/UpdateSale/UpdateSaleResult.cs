using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public record UpdateSaleResult(
    int Id,
    Guid UserId,
    DateTime SaleDate,
    List<SaleItem> SaleItems,
    decimal TotalSaleAmount,
    decimal TotalSaleDiscount,
    string Branch,
    bool IsCanceled)
{
    public UpdateSaleResult() : this(
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