using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public record CancelSaleCommand(int SaleId) : IRequest<CancelSaleResult>
{
    public CancelSaleCommand() : this(0) { }
    public static CancelSaleCommand Create(int saleId) => new CancelSaleCommand(saleId);
}