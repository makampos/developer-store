using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.ValueObjects.TestData;

public static class CarItemTestData
{
    private static readonly Faker<CartItem> cartItemFaker = new Faker<CartItem>()
        .RuleFor(x => x.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(x => x.ProductId, f => f.Random.Guid());

    public static CartItem GenerateValidCartItem(Guid productId, int quantity)
    {
        return cartItemFaker
            .RuleFor(x => x.ProductId, _ => productId)
            .RuleFor(x => x.Quantity, _ => quantity)
            .Generate();
    }
}