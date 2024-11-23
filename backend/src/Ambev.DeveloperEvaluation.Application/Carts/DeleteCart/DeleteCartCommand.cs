using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public record DeleteCartCommand(Guid Id) : IRequest<DeleteCartResult>
{
    public static DeleteCartCommand Create(Guid id)
    {
        return new DeleteCartCommand(id);
    }
}
