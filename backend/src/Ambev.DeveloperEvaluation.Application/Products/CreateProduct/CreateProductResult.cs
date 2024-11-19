namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public record CreateProductResult(Guid Id)
{
   public static CreateProductResult Create(Guid id) => new CreateProductResult(id);
}