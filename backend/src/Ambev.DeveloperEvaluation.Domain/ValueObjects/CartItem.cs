namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public record CartItem(Guid ProductId, int Quantity)
{
    public static CartItem Create(Guid productId, int quantity) => new(productId, quantity);

    // parameterless constructor for Faker
    public CartItem() : this(Guid.Empty, 0)
    {
    }
}
