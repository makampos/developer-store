using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

public record GetAllProductsCommand(int PageNumber, int PageSize) : IRequest<GetAllProductsResult>
{
    public static GetAllProductsCommand Create(int pageNumber, int pageSize)
    {
        return new GetAllProductsCommand(pageNumber, pageSize);
    }
}