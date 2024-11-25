using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public record UpdateSaleRequest(
    int Id,
    Guid UserId,
    List<SaleItem> SaleItems,
    string Branch)
{
    public UpdateSaleRequest IncludeId(int id)
    {
        return this with { Id = id };
    }
}