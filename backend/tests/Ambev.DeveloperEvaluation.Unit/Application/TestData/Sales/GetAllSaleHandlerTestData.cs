using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

public static class GetAllSaleHandlerTestData
{
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .RuleFor(s => s.Id, f => f.Random.Int())
        .RuleFor(s => s.UserId, f => f.Random.Guid())
        .RuleFor(s => s.SaleItems, f => f.Make(1, () => new SaleItem
        {
            ProductId = f.Random.Guid(),
            Quantity = f.Random.Int(1, 10),
            UnitPrice = f.Random.Decimal(1, 100)
        }))
        .RuleFor(s => s.SaleDate, f => f.Date.Past())
        .RuleFor(s => s.TotalSaleAmount, f => f.Random.Decimal(1, 1000))
        .RuleFor(s => s.TotalSaleDiscount, f => f.Random.Decimal(1, 100))
        .RuleFor(s => s.Branch, f => f.Random.String2(10))
        .RuleFor(s => s.IsCanceled, f => f.Random.Bool());

    public static List<Sale> GetAllSales(int count)
    {
        return SaleFaker.Generate(count);
    }
}