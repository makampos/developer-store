using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.CartItems, f => f.Make(3, ()
            => CartItem.Create(f.Random.Guid(), f.Random.Int(1, 10))))
        .RuleFor(x => x.Branch, f => f.Random.String2(10));

    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleCommandFaker.Generate();
    }

    public static CreateSaleCommand GenerateValidCommandWithValidCartItems(Guid productId, int quantity)
    {
        return createSaleCommandFaker
            .RuleFor(x => x.CartItems, f => f.Make(1, ()
                => CartItem.Create(productId, quantity))) // override CartItems
            .Generate();
    }
}