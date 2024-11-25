namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public record CancelSaleResult(string Message)
{
    public static CancelSaleResult Create(string message) => new CancelSaleResult(message);
}