using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public record GetCartCommand(Guid Id) : IRequest<GetCartResult>
{
    // parameter less constructor for automapper
    public GetCartCommand() : this(Guid.Empty)
    {
    }
}