using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public record UpdateCartRequest(Guid Id, List<CartItem> CartItems)
{
    public UpdateCartRequest() : this(Guid.Empty, []) { }

    public static UpdateCartRequest Create(Guid id, List<CartItem> cartItems) =>
        new UpdateCartRequest(id, cartItems);

    public UpdateCartRequest IncludeId(Guid id)
    {
        return this with { Id = id };
    }
}