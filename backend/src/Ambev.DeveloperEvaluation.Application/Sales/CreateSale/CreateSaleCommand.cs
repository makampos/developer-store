using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public record CreateSaleCommand(
    Guid UserId,
    List<CartItem> CartItems,
    string Branch) : IRequest<CreateSaleResult>
{
    // parameterless for faker
    public CreateSaleCommand() : this(Guid.Empty, [], string.Empty)
    { }

}