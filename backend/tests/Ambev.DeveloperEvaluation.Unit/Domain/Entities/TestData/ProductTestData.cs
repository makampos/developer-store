using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class ProductTestData
{
    private static Faker<Product> productFaker = new Faker<Product>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Title, f => f.Commerce.ProductName())
        .RuleFor(x => x.Price, f => Math.Round(f.Finance.Random.Decimal(1, 500), 1))
        .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
        .RuleFor(x => x.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(x => x.Image, f => f.Image.PicsumUrl())
        .RuleFor(x => x.Rating, f => Rating.Create(f.Random.Int(1, 5), f.Random.Int(1, 100)));

    public static Product GenerateValidProduct(decimal price)
    {
        return productFaker
            .RuleFor(x => x.Price, _ => price)
            .Generate();
    }
}