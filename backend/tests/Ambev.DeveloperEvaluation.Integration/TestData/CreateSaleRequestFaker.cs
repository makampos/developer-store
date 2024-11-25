using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

public static class CreateSaleRequestFaker
{
    private static readonly Faker<CreateSaleRequest> _faker = new Faker<CreateSaleRequest>()
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.CartItems, f => f.Make(3, ()
            => CartItem.Create(f.Random.Guid(), f.Random.Int(1, 10))))
        .RuleFor(x => x.Branch, f => f.Random.String2(10));

    public static CreateSaleRequest GenerateValidRequest(List<Guid> productIds)
    {
        return _faker
            .RuleFor(x => x.CartItems, f => f.Make<CartItem>(productIds.Count, (i)
                =>
            {
                var productId = productIds[i - 1];
                return CartItem.Create(productId, f.Random.Int(1, 10));
            }))
            .Generate();
    }
}