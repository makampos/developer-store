using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public record CartItemDetails : CartItem
{
    public decimal UnitPrice { get; private set; }

    public decimal TotalAmountWithDiscount { get; private set; }
    public decimal TotalAmount { get; set; }
    public Product Product { get; private set; }
    public decimal TotalDiscounts { get; private set; }

    public CartItemDetails(CartItem cartItem)
    {
        Quantity = cartItem.Quantity;
        ProductId = cartItem.ProductId;
    }

    public CartItemDetails ToCartItemDetails(Product product)
    {
        var totalAmountWithDiscount = CalculateTotal(product.Price);

        TotalAmountWithDiscount = totalAmountWithDiscount;
        UnitPrice = product.Price;
        Product = product;
        TotalDiscounts = totalAmountWithDiscount - Quantity * product.Price;
        TotalAmount = Quantity * product.Price;

        return this;
    }

    private decimal CalculateTotal(decimal unitPrice)
    {
        decimal discount = 0;

        // Maximum limit: 20 items per identical product
        if (Quantity > 20)
        {
            throw new DomainException("Maximum limit of 20 items per product");
        }

        // No discounts allowed for quantities below 4 items
        // 4+ items: 10% discount
        // 10-20 items: 20% discount
        if (Quantity >= 4 && Quantity <= 20)
        {
            if (Quantity >= 10)
            {
                discount = 0.20m; // 20% discount
            }
            else
            {
                discount = 0.10m; // 10% discount
            }
        }

        return Quantity * unitPrice * (1 - discount);
    }
}