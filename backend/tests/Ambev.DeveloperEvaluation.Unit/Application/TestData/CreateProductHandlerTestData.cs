using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CreateProductHandlerTestData
{
    private static readonly Faker<CreateProductCommand> createProductHandlerFaker = new Faker<CreateProductCommand>()
        .RuleFor(x => x.Title, f => f.Commerce.ProductName())
        .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
        .RuleFor(x => x.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(x => x.Image, f => f.Image.PicsumUrl())
        .RuleFor(x => x.Rating, f => Rating.Create( f.Random.Int(1, 5),
            f.Random.Int(1, 100)));

    public static CreateProductCommand GenerateValidCommand()
    {
        return createProductHandlerFaker.Generate();
    }
}