using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public record SaleModifiedEvent(
    int Id,
    Guid UserId,
    DateTime SaleDate,
    List<SaleItem> SaleItems,
    decimal TotalSaleAmount,
    decimal TotalSaleDiscount,
    string Branch,
    bool IsCanceled)
{
    public static SaleModifiedEvent Build(int id, Guid userId, DateTime saleDate, List<SaleItem> saleItems,
        decimal totalSaleAmount, decimal totalSaleDiscount, string branch, bool isCanceled)
    {
        return new SaleModifiedEvent(id, userId, saleDate, saleItems, totalSaleAmount, totalSaleDiscount, branch, isCanceled);
    }

    public override string ToString()
    {
        return $"Sale Id: {Id}, " +
               $"UserId: {UserId}, " +
               $"SaleDate: {SaleDate}, " +
               $"TotalSaleAmount: {TotalSaleAmount}, " +
               $"TotalSaleDiscount: {TotalSaleDiscount}, " +
               $"Branch: {Branch}, " +
               $"IsCanceled: {IsCanceled} " +
               $"Sale Items: {string.Join(", ", SaleItems.Select(
                 saleItem =>
                 "Product Id: " + saleItem.ProductId +
                 ", Quantity: " + saleItem.Quantity +
                 ", Price: " + saleItem.UnitPrice +
                 ", Total Amount With Discount: " + saleItem.TotalAmountWithDiscount +
                 ", Total Amount: " + saleItem.TotalSaleItemAmount))}";
    }
}