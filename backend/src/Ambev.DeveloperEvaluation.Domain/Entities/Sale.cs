using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale
{
    public int Id { get;  set; } // Sale number
    public Guid UserId { get; private set; } // Customer who made the purchase
    public DateTime SaleDate { get; private set; } // Date when the sale was made
    public List<SaleItem> SaleItems { get; private set; } = []; // Products
    public decimal TotalSaleAmount { get; private set; } // Total sale amount
    public bool IsCanceled { get; private set; } // Cancelled/Not Cancelled

    public decimal TotalSaleDiscount { get; private set; } // Discount applied to the sale
    public string Branch { get; private set; } // Branch where the sale was made

    public Sale() { }

    public void BuildSale(IReadOnlyCollection<CartItemDetails> cartItems, IReadOnlyCollection<Product> products, string branch,
        Guid userId)
    {
        foreach (var item in cartItems)
        {
            var product = GetProductById(item.ProductId); // Fetch product details (example, price)
            var saleItem = new SaleItem(product.Id, item.Quantity, product.Price, item.TotalAmountWithDiscount, item.TotalAmount);
            SaleItems.Add(saleItem);
            TotalSaleAmount += saleItem.TotalAmountWithDiscount;
        }

        SaleDate = DateTime.Now;
        Branch = branch;
        UserId = userId;

        // calculate total discount by looking at the total discount of each item
        TotalSaleDiscount = cartItems.Sum(x => (x.UnitPrice * x.Quantity) - x.TotalAmountWithDiscount!);

        return;

        Product GetProductById(Guid productId)
        {
            // at this point, we are sure that product exists
            return products.FirstOrDefault(p => p.Id == productId)!;
        }
    }

    public void CancelSale()
    {
        if (IsCanceled)
        {
            throw new DomainException("Sale already canceled");
        }

        IsCanceled = true;
    }
}

