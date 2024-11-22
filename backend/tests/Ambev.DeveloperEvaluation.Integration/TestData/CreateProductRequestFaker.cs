using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

public class CreateProductRequestFaker
{
    private static readonly Faker<CreateProductRequest> createProductRequestFaker = new Faker<CreateProductRequest>()
        .RuleFor(x => x.Title, f => f.Commerce.ProductName())
        .RuleFor(x => x.Price, f => f.Finance.Amount())
        .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
        .RuleFor(x => x.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(x => x.Image, f => f.Image.PicsumUrl())
        .RuleFor(x => x.Rating, f => Rating.Create( f.Random.Int(1, 5),
            f.Random.Int(1, 100)));

    public static CreateProductRequest GenerateInvalidRequest()
    {
        return createProductRequestFaker
            .RuleFor(x => x.Title, f => string.Empty)
            .Generate();
    }

    public static List<CreateProductRequest> GenerateValidRequests(int count)
    {
        return createProductRequestFaker.Generate(count);
    }

    public static List<CreateProductRequest> GenerateValidRequestsWithTies(int count)
    {
        var price = createProductRequestFaker.Generate().Price;
        return createProductRequestFaker
            .RuleFor(x => x.Price, f => f.Finance.Amount(price, price))
            .Generate(count);
    }
}