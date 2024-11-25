using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

public static class GetSaleHandlerTestData
{
    private static readonly Faker<GetSaleCommand> getSaleCommandFaker = new Faker<GetSaleCommand>()
        .RuleFor(x => x.Id, f => f.Random.Int(1, 100));

    public static GetSaleCommand GenerateValidCommand()
    {
        return getSaleCommandFaker.Generate();
    }

    private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
        .RuleFor(x => x.Id, f => f.Random.Int(1, 100))
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.SaleDate, f => f.Date.Past())
        .RuleFor(x => x.SaleItems, f => f.Make<SaleItem>(1, ()
            => SaleItem.Create(
                   productId: f.Random.Guid(),
                   quantity: f.Random.Int(1, 10),
                   unitPrice: f.Random.Decimal(1, 10),
                   totalAmountWithDiscount: f.Random.Decimal(1, 10),
                   totalSaleItemAmount: f.Random.Decimal(1, 10))))
        .RuleFor(x => x.TotalSaleAmount, f => f.Random.Decimal(1, 1000))
        .RuleFor(x => x.TotalSaleDiscount, f => f.Random.Decimal(1, 100))
        .RuleFor(x => x.Branch, f => f.Random.String2(10))
        .RuleFor(x => x.IsCanceled, _ => false);

    public static Sale GenerateValidSale(Guid productId, int saleItemsCount, int saleItemQuantity)
    {
        return saleFaker
            .RuleFor(x => x.SaleItems, f => f.Make(saleItemsCount, () => SaleItem.Create(
                    productId: productId,
                    quantity: saleItemQuantity,
                    unitPrice: f.Random.Decimal(1, 10),
                    totalAmountWithDiscount: f.Random.Decimal(1, 10),
                    totalSaleItemAmount: f.Random.Decimal(1, 10)))) // override SaleItems
            .Generate();
    }

    public static Sale GenerateSaleWithSelectedSaleItems(List<SaleItem> selectedSaleItem)
    {
        return saleFaker
            .RuleFor(x => x.SaleItems, selectedSaleItem) // override SaleItems
            .Generate();
    }
}