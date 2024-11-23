namespace Ambev.DeveloperEvaluation.WebApi.Carts.GetCart;

public record GetCartRequest(Guid Id)
{
    public static GetCartRequest Create(Guid id) => new GetCartRequest(id);
}