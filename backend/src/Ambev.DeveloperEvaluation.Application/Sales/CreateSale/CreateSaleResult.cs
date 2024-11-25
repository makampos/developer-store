using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public record CreateSaleResult(
    int Id,
    Guid UserId,
    DateTime SaleDate,
    List<SaleItem> SaleItems,
    decimal TotalSaleAmount,
    decimal TotalSaleDiscount,
    string Branch,
    bool IsCanceled
)
{
    public static CreateSaleResult CreateSaleResultFromSale(Sale sale)
    {
        return new CreateSaleResult(
            sale.Id,
            sale.UserId,
            sale.SaleDate,
            sale.SaleItems,
            sale.TotalSaleAmount,
            sale.TotalSaleDiscount,
            sale.Branch,
            sale.IsCanceled
        );
    }
}