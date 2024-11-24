using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

public record GetCartResponse(Guid Id, Guid UserId, DateTime Date, List<CartItem> CartItems);