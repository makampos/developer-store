using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public record CreateCartResult(Guid Id, Guid UserId, DateTime Date, List<CartItem> CartItems)
{
    // parameterless for automapper
    public CreateCartResult() : this(Guid.Empty, Guid.Empty, DateTime.MinValue, new List<CartItem>())
    {
    }

    public static CreateCartResult Create(Guid id, Guid userId, DateTime date, List<CartItem> cartItems)
    {
        return new CreateCartResult(id, userId, date, cartItems);
    }
}