using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Carts.UpdateCart;

public record UpdateCartResponse(Guid Id, Guid UserId, DateTime Date, List<CartItem> CartItems);