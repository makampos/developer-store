namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public record GetSaleRequest(int Id)
{
    public static GetSaleRequest Create(int id)
    {
        return new GetSaleRequest(id);
    }
}