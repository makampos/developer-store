namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class SaleItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalAmountWithDiscount { get; set; }
    public decimal TotalSaleItemAmount { get; set; }

    public SaleItem(Guid productId, int quantity, decimal unitPrice, decimal totalAmountWithDiscount, decimal totalSaleItemAmount)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalAmountWithDiscount = totalAmountWithDiscount;
        TotalSaleItemAmount = totalSaleItemAmount;
    }
}