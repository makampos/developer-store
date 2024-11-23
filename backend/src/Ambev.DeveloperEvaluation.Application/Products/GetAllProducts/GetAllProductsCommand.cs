using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

public record GetAllProductsCommand(int PageNumber, int PageSize, string? Order = null) : IRequest<GetAllProductsResult>
{
    public static GetAllProductsCommand Create(int pageNumber, int pageSize, string? order = null)
    {
        return new GetAllProductsCommand(pageNumber, pageSize, order);
    }
}