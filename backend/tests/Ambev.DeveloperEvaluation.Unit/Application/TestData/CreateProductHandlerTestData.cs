using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CreateProductHandlerTestData
{
    private static readonly Faker<CreateProductCommand> createProductHandlerFaker = new Faker<CreateProductCommand>()
        .RuleFor(x => x.Title, f => f.Commerce.ProductName())
        .RuleFor(x => x.Price, f => Math.Round(f.Finance.Random.Decimal(1, 500), 1))
        .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
        .RuleFor(x => x.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(x => x.Image, f => f.Image.PicsumUrl())
        .RuleFor(x => x.Rating, f => Rating.Create( f.Random.Int(1, 5),
            f.Random.Int(1, 100)));

    private static readonly Faker<Product> productFaker = new Faker<Product>();

    public static Product GenerateValidProduct(CreateProductCommand command, Guid productId)
    {
        return productFaker
            .RuleFor(x => x.Id, _ => productId)
            .RuleFor(x => x.Title, _ => command.Title)
            .RuleFor(x => x.Price, _ => command.Price)
            .RuleFor(x => x.Description, _ => command.Description)
            .RuleFor(x => x.Category, _ => command.Category)
            .RuleFor(x => x.Image, _ => command.Image)
            .RuleFor(x => x.Rating, _ => command.Rating)
            .Generate();
    }

    public static CreateProductCommand GenerateValidCommand()
    {
        return createProductHandlerFaker.Generate();
    }
}