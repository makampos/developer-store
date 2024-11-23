namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public record CartItem(Guid ProductId, int Quantity)
{
    public static CartItem Create(Guid productId, int quantity) => new CartItem(productId, quantity);
}
