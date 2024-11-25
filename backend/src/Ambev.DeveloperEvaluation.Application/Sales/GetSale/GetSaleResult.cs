using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public record GetSaleResult(
    int Id,
    Guid UserId,
    DateTime SaleDate,
    List<SaleItem> SaleItems,
    decimal TotalSaleAmount,
    decimal TotalSaleDiscount,
    string Branch,
    bool IsCanceled)
{
    public static GetSaleResult CreateGetSaleResultFromSale(Sale sale)
    {
        return new GetSaleResult(
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