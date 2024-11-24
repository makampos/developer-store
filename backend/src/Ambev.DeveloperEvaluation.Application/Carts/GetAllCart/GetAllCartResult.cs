using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetAllCart;

public record GetAllCartResult(PagedResult<GetCartResult> Carts)
{
    // for automapper
    public GetAllCartResult() : this(new PagedResult<GetCartResult>())
    {

    }
}