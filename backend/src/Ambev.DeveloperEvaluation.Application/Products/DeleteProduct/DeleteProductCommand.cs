using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<DeleteProductResponse>
{
    public static DeleteProductCommand Create(Guid id)
    {
        return new DeleteProductCommand(id);
    }
}