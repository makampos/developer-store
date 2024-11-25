using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public record UpdateSaleCommand(
    int Id,
    Guid UserId,
    List<SaleItem> SaleItems,
    string Branch
    ) : IRequest<UpdateSaleResult>
{
    public UpdateSaleCommand() : this(
        0,
        Guid.Empty,
        [],
        string.Empty)
    {
    }
}