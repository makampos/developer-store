using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public record CreateCartRequest(Guid UserId, DateTime Date, List<CartItem> CartItems)
{
    // parameterless for faker
    public CreateCartRequest() : this(Guid.Empty, DateTime.MinValue, new List<CartItem>())
    {
    }
}