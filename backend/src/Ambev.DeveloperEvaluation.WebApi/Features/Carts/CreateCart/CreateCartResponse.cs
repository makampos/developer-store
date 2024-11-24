using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public record CreateCartResponse(Guid Id, Guid UserId, DateTime Date, List<CartItem> CartItems)
{

}