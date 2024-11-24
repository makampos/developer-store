using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public record UpdateCartResult(Guid Id, Guid UserId, DateTime Date, List<CartItem> CartItems)
{
    // parameterless for automapper
    public UpdateCartResult() : this(Guid.Empty, Guid.Empty, DateTime.MinValue, [])
    {
    }

    public static UpdateCartResult Create(Guid id, Guid userId, DateTime date, List<CartItem> cartItems)
    {
        return new UpdateCartResult(id, userId, date, cartItems);
    }
}