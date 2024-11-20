using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Products;

public static class UpdateProductHandlerTestData
{
    private static Faker<UpdateProductCommand> updateProductHandlerFaker = new Faker<UpdateProductCommand>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Title, f => f.Commerce.ProductName())
        .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
        .RuleFor(x => x.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(x => x.Image, f => f.Image.PicsumUrl())
        .RuleFor(x => x.Rating, f => Rating.Create( f.Random.Int(1, 5),
            f.Random.Int(1, 100)));

    public static UpdateProductCommand GenerateValidCommand()
    {
        return updateProductHandlerFaker.Generate();
    }
}