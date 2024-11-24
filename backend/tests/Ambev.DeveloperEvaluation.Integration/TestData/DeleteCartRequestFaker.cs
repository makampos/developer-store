using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

public class DeleteCartRequestFaker
{
    private static readonly Faker<DeleteCartRequest> deleteCartRequestFaker = new Faker<DeleteCartRequest>()
        .RuleFor(x => x.Id, f => Guid.Empty);

    public static DeleteCartRequest GenerateInvalidRequest()
    {
        return deleteCartRequestFaker.Generate();
    }
}