using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

public class UpdateProductRequestFaker
{
    private static readonly Faker<UpdateProductRequest> updateProductRequestFaker = new Faker<UpdateProductRequest>()
        .RuleFor(x => x.Title, f => f.Commerce.ProductName())
        .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
        .RuleFor(x => x.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(x => x.Image, f => f.Image.PicsumUrl())
        .RuleFor(x => x.Rating, f => Rating.Create( f.Random.Int(1, 5),
            f.Random.Int(1, 100)));

    public static UpdateProductRequest GenerateValidRequest()
    {
        return updateProductRequestFaker.Generate();
    }

    public static UpdateProductRequest GenerateInvalidRequest()
    {
        return updateProductRequestFaker
            .RuleFor(x => x.Title, f => string.Empty) // override Title to be empty
            .Generate();
    }
}