using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public record CreateSaleRequest(Guid UserId, List<CartItem> CartItems, string Branch)
{
    // parameterless for faker
    public CreateSaleRequest() : this(Guid.Empty, [], string.Empty)
    {
    }
}