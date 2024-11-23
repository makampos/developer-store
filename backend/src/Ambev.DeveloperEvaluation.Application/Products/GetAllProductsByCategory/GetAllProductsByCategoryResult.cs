using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProductsByCategory;

public record GetAllProductsByCategoryResult(PagedResult<GetProductResult> Products)
{
    // for automapper
    public GetAllProductsByCategoryResult() : this(new PagedResult<GetProductResult>())
    {

    }
}