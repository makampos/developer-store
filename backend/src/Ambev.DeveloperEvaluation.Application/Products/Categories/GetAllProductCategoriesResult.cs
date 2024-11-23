namespace Ambev.DeveloperEvaluation.Application.Products.Categories;

public record GetAllProductCategoriesResult(IReadOnlyList<string> ProductCategories)
{
    public static GetAllProductCategoriesResult Create(IReadOnlyList<string> productCategories)
    {
        return new GetAllProductCategoriesResult(productCategories);
    }
}