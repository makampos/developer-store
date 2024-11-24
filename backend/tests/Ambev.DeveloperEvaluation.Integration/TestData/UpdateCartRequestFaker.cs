using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

public static class UpdateCartRequestFaker
{
    private static readonly Faker<UpdateCartRequest> _updateCartRequestFaker = new Faker<UpdateCartRequest>()
        .RuleFor(x => x.CartItems, f => f.Make(3, ()
            => CartItem.Create(f.Random.Guid(), f.Random.Int(1, 10))));

    public static UpdateCartRequest GenerateValidRequest()
    {
        return _updateCartRequestFaker.Generate();
    }

    public static UpdateCartRequest GenerateInvalidRequest()
    {
        return new UpdateCartRequest();
    }
}