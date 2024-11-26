using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public record SaleCreatedEvent(
    int Id ,
    Guid UserId,
    DateTime SaleDate,
    List<SaleItem> SaleItems,
    decimal TotalSaleAmount,
    bool IsCanceled,
    decimal TotalSaleDiscount,
    string Branch)
{
    public static SaleCreatedEvent Build(int id, Guid userId, DateTime saleDate, List<SaleItem> saleItems, decimal
            totalSaleAmount, bool isCanceled, decimal totalSaleDiscount, string branch)
    {
        return new SaleCreatedEvent(id, userId, saleDate, saleItems, totalSaleAmount, isCanceled, totalSaleDiscount, branch);
    }

    public override string ToString()
    {
        return $"Sale Id: {Id}, " +
               $"User Id: {UserId}, " +
               $"Sale Date: {SaleDate}," +
               $"Total Sale Amount: {TotalSaleAmount}, " +
               $"Is Canceled: {IsCanceled}, " +
               $"Total Sale Discount: {TotalSaleDiscount}, " +
               $"Branch: {Branch}" +
               $"Sale Items: {string.Join(", ", SaleItems.Select(
                   saleItem =>
                    "Product Id: " + saleItem.ProductId +
                    ", Quantity: " + saleItem.Quantity +
                    ", Price: " + saleItem.UnitPrice +
                    ", Total Amount With Discount: " + saleItem.TotalAmountWithDiscount +
                    ", Total Amount: " + saleItem.TotalSaleItemAmount))}";
    }
}