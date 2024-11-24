using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

public class CreateCartRequestFaker
{
    private static readonly Faker<CreateCartRequest> createCartRequestFaker = new Faker<CreateCartRequest>()
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.Date, f => f.Date.Past())
        .RuleFor(x => x.CartItems, f => f.Make(3, () => CartItem.Create(
            f.Random.Guid(), f.Random.Int(1, 10))));

    public static CreateCartRequest GenerateValidRequest()
    {
        return createCartRequestFaker.Generate();
    }

    public static List<CreateCartRequest> GenerateValidRequests(int count)
    {
        return createCartRequestFaker.Generate(count);
    }
}