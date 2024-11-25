using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

public static class UpdateSaleRequestFaker
{
    private static readonly Faker<UpdateSaleCommand> updateSaleCommandFaker = new Faker<UpdateSaleCommand>()
        .RuleFor(x => x.Id, f => f.Random.Int(1))
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.SaleItems, f => f.Make(1, () =>
            SaleItem.Create(
                productId: f.Random.Guid(),
                quantity: f.Random.Int(1, 10),
                unitPrice: f.Random.Decimal(1, 10),
                totalAmountWithDiscount: f.Random.Decimal(1, 10),
                totalSaleItemAmount: f.Random.Decimal(1, 10)))
        )
        .RuleFor(x => x.Branch, f => f.Random.String2(1, 50));

    public static UpdateSaleCommand GenerateValidRequest(int saleId, Guid productId)
    {
        return updateSaleCommandFaker
            .RuleFor(x => x.Id, _ => saleId)
            .RuleFor(x => x.SaleItems, f =>
                f.Make<SaleItem>(1, () =>
                    SaleItem.Create(
                        productId: productId,
                        quantity: f.Random.Int(1, 10),
                        unitPrice: f.Random.Decimal(1, 10),
                        totalAmountWithDiscount: f.Random.Decimal(1, 10),
                        totalSaleItemAmount: f.Random.Decimal(1, 10))))
            .RuleFor(x => x.Branch, f => f.Random.String2(1, 50))
            .Generate();
    }
}