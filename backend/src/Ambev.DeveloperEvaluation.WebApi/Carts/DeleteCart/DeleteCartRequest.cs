namespace Ambev.DeveloperEvaluation.WebApi.Carts.DeleteCart;

public record DeleteCartRequest(Guid Id)
{

    public DeleteCartRequest() : this(Guid.Empty)
    {
    }

    public static DeleteCartRequest Create(Guid id) => new(id);
}