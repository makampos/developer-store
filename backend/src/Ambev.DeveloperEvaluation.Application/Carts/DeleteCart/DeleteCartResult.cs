namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public record DeleteCartResult(bool Success)
{
    public static DeleteCartResult Create(bool success)
    {
        return new DeleteCartResult(success);
    }
}
