using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public record GetCartResult(Guid Id, Guid UserId, DateTime Date, List<CartItem> CartItems)
{
    // parameterless for automapper
    public GetCartResult() : this(Guid.Empty, Guid.Empty, DateTime.MinValue, new List<CartItem>())
    {
    }

    public static GetCartResult Create(Guid id, Guid userId, DateTime date, List<CartItem> cartItems)
    {
        return new GetCartResult(id, userId, date, cartItems);
    }
}