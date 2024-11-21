using Ambev.DeveloperEvaluation.Domain.Entities;


namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

public record GetAllProductsResult(PagedResult<Product> Products)
{
    public GetAllProductsResult() : this(new PagedResult<Product>())
    {

    }
}