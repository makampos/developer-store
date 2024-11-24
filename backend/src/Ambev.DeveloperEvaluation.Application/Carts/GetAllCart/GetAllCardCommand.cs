using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetAllCart;

public record GetAllCardCommand(int PageNumber, int PageSize, string? Order = null) : IRequest<GetAllCartResult>
{
    public static GetAllCardCommand Create(int pageNumber, int pageSize, string? order = null)
    {
        return new GetAllCardCommand(pageNumber, pageSize, order);
    }
}