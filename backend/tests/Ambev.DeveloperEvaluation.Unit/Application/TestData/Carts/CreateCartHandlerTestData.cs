using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Carts;

public static class CreateCartHandlerTestData
{
    private static readonly Faker<CreateCartCommand> createCartCommandFaker = new Faker<CreateCartCommand>()
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.Date, f => DateTime.Now)
        .RuleFor(x => x.CartItems, f => f.Make(3, () => CartItem.Create(
            f.Random.Guid(), f.Random.Int(1, 10))));

    public static CreateCartCommand GenerateValidCommand()
    {
        return createCartCommandFaker.Generate();
    }
}