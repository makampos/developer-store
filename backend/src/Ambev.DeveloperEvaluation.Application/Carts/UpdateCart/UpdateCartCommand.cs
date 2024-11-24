using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public record UpdateCartCommand(Guid Id, List<CartItem> CartItems) : IRequest<UpdateCartResult>
{
    // Necessary for Faker
    public UpdateCartCommand() : this(Guid.NewGuid(), [])
    {
    }
}