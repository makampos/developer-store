namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public record CancelSaleRequest(int Id)
{
    public static CancelSaleRequest Create(int id)
    {
        return new CancelSaleRequest(id);
    }
}