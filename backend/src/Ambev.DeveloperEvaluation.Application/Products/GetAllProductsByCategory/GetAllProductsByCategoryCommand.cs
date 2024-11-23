using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProductsByCategory;

public record GetAllProductsByCategoryCommand(string Category, int PageNumber, int PageSize, string? Order = null)
    : IRequest<GetAllProductsByCategoryResult>
{
    public static GetAllProductsByCategoryCommand Create(string category, int pageNumber, int pageSize,
        string? order = null)
    {
        return new GetAllProductsByCategoryCommand(category, pageNumber, pageSize, order);
    }
}