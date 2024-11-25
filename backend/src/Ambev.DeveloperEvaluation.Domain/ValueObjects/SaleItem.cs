namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public record SaleItem(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalAmountWithDiscount,
    decimal TotalSaleItemAmount)
{
    public SaleItem() : this(
        Guid.Empty,
        0,
        0,
        0,
        0)
    {
    }

    public static SaleItem Create(
        Guid productId,
        int quantity,
        decimal unitPrice,
        decimal totalAmountWithDiscount,
        decimal totalSaleItemAmount)
    {
        return new SaleItem(
            productId,
            quantity,
            unitPrice,
            totalAmountWithDiscount,
            totalSaleItemAmount);
    }
}
