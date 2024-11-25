using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

public static class UpdateSaleHandlerTestData
{
    private static readonly Faker<UpdateSaleCommand> _updateSaleCommandFaker = new Faker<UpdateSaleCommand>()
        .RuleFor(x => x.Id, f => f.Random.Int(1, 100))
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.Branch, f => f.Random.String2(10))
        .RuleFor(x => x.SaleItems, f => f.Make<SaleItem>(1, ()
            => SaleItem.Create(
                productId: f.Random.Guid(),
                quantity: f.Random.Int(1, 10),
                unitPrice: f.Random.Decimal(1, 10),
                totalAmountWithDiscount: f.Random.Decimal(1, 10),
                totalSaleItemAmount: f.Random.Decimal(1, 10)))
        );

    public static UpdateSaleCommand GenerateValidCommand(int saleId, List<Guid> productIds, int saleItemCount)
    {
        return _updateSaleCommandFaker
            .RuleFor(x => x.Id, _ => saleId)
            .RuleFor(x => x.SaleItems, f => f.Make<SaleItem>(saleItemCount, ()
                => SaleItem.Create(
                    productId: productIds[f.Random.Int(0, productIds.Count - 1)],
                    quantity: f.Random.Int(1, 10),
                    unitPrice: f.Random.Decimal(1, 10),
                    totalAmountWithDiscount: f.Random.Decimal(1, 10),
                    totalSaleItemAmount: f.Random.Decimal(1, 10)))
            )
            .Generate();
    }

    private static readonly Faker<UpdateSaleResult> _updateSaleResultFaker = new Faker<UpdateSaleResult>()
        .RuleFor(x => x.Id, f => f.Random.Int(1, 100))
        .RuleFor(x => x.UserId, f => f.Random.Guid())
        .RuleFor(x => x.Branch, f => f.Random.String2(10))
        .RuleFor(x => x.SaleItems, f => f.Make<SaleItem>(1, ()
            => SaleItem.Create(
                productId: f.Random.Guid(),
                quantity: f.Random.Int(1, 10),
                unitPrice: f.Random.Decimal(1, 10),
                totalAmountWithDiscount: f.Random.Decimal(1, 10),
                totalSaleItemAmount: f.Random.Decimal(1, 10)))
        );

    public static UpdateSaleResult GenerateValidResult(Sale sale)
    {
        return _updateSaleResultFaker
            .RuleFor(x => x.Id, _ => sale.Id)
            .RuleFor(x => x.UserId, _ => sale.UserId)
            .RuleFor(x => x.Branch, _ => sale.Branch)
            .RuleFor(x => x.SaleItems, _ => sale.SaleItems)
            .RuleFor(x => x.TotalSaleAmount, _ => sale.TotalSaleAmount)
            .RuleFor(x => x.TotalSaleDiscount, _ => sale.TotalSaleDiscount)
            .RuleFor(x => x.SaleDate, _ => sale.SaleDate)
            .RuleFor(x => x.IsCanceled, _ => sale.IsCanceled)
            .Generate();
    }

    public static UpdateSaleCommand GenerateInvalidCommand()
    {
        return _updateSaleCommandFaker
            .RuleFor(x => x.Id, _ => 0)
            .Generate();
    }
}