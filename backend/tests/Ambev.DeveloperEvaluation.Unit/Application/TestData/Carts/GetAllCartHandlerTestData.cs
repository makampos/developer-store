using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Carts;

public static class GetAllCartHandlerTestData
{
    private static readonly Faker<Cart> cartFaker = new Faker<Cart>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.UserId, f => f.Random.Guid())
        .RuleFor(c => c.Date, f => f.Date.Past())
        .RuleFor(c => c.Products, f => f.Make(10,
            () => CartItem.Create(f.Random.Guid(), f.Random.Int(1, 10))));

    public static List<Cart> GetAllCarts(int count)
    {
        return cartFaker.Generate(count);
    }
}