using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

public static class GetAllProductsHandlerTestData
{
    private static Faker<Product> productFaker = new Faker<Product>()
        .RuleFor(p => p.Title, f => f.Commerce.ProductName())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
        .RuleFor(p => p.Rating, f => Rating.Create(f.Random.Int(1, 5), f.Random.Int(1, 100)));

    public static List<Product> GetAllProducts(int count)
    {
        return productFaker.Generate(count);
    }
}