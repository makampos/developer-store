using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public record CreateCartCommand(Guid UserId, DateTime Date, List<CartItem> CartItems) : IRequest<CreateCartResult>
{
    // Necessary for Faker
    public CreateCartCommand() : this(Guid.NewGuid(), DateTime.Now, [])
    {
    }
}