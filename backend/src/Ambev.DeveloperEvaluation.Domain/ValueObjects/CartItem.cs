namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public record CartItem(Guid ProductId, int Quantity)
{
    public static CartItem Create(Guid productId, int quantity) => new CartItem(productId, quantity);
    public static bool IsValid(CartItem cartItem)
    {
        return cartItem.ProductId != Guid.Empty && cartItem.Quantity > 0;
    }
}
