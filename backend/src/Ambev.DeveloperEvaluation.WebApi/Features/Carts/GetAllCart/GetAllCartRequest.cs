namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetAllCart;

public record GetAllCartRequest(int? PageNumber = 1, int? PageSize = 10, string? Order = null)
{
    public static GetAllCartRequest Create(int pageNumber, int pageSize, string? order = null) =>
        new GetAllCartRequest(pageNumber, pageSize, order);
}