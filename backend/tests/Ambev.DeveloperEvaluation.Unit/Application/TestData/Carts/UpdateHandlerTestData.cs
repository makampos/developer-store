using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Carts;

public static class UpdateHandlerTestData
{
    private static readonly Faker<UpdateCartCommand> _updateCartCommandFaker = new Faker<UpdateCartCommand>()
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.CartItems, f => f.Make(3, ()
            => CartItem.Create(f.Random.Guid(), f.Random.Int(1, 10))));

    private static readonly Faker<Cart> cartFaker = new Faker<Cart>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.UserId, f => f.Random.Guid())
        .RuleFor(c => c.Date, f => f.Date.Past())
        .RuleFor(c => c.Products, f => f.Make(3,
            () => CartItem.Create(f.Random.Guid(), f.Random.Int(1, 10))));

    public static UpdateCartCommand GenerateValidCommand()
    {
        return _updateCartCommandFaker.Generate();
    }

    public static Cart GenerateCart()
    {
        return cartFaker.Generate();
    }
}