using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

public record GetAllSaleCommand(int PageNumber, int PageSize, string? Order = null) : IRequest<GetAllSaleResult>
{
    public static GetAllSaleCommand Create(int pageNumber, int pageSize, string? order = null)
    {
        return new GetAllSaleCommand(pageNumber, pageSize, order);
    }
}