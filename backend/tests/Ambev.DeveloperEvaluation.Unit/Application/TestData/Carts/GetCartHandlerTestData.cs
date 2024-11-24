using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Carts;

public static class GetCartHandlerTestData
{
    private static readonly Faker<GetCartCommand> createCartHandlerFaker = new Faker<GetCartCommand>()
        .RuleFor(x => x.Id, f => f.Random.Guid());

    private static readonly Faker<Cart> createCartFaker = new Faker<Cart>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.Date, f => f.Date.Past())
        .RuleFor(x => x.Products, f => f.Make(3, () =>
            CartItem.Create( f.Random.Guid(), f.Random.Int(1, 10))));

    public static GetCartCommand GenerateValidCommand()
    {
        return createCartHandlerFaker.Generate();
    }

    public static Cart GenerateValidCart()
    {
        return createCartFaker.Generate();
    }
}