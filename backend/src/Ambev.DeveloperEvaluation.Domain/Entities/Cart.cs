using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }
    public List<CartItem> Products { get; private set; } = new List<CartItem>();

    public Cart()
    {

    }

    public static Cart Create(Guid userId, DateTime date, List<CartItem> products)
    {
        return new Cart
        {
            UserId = userId,
            Date = date,
            Products = products
        };
    }

    public void AddProduct(Guid productId, int quantity)
    {
        Products.Add(new CartItem(productId, quantity));
    }

    public void RemoveProduct(Guid productId)
    {
        var product = Products.FirstOrDefault(x => x.ProductId == productId);
        if (product != null)
        {
            Products.Remove(product);
        }
    }

    public void RemoveAllProducts(Guid productId)
    {
        Products.RemoveAll(x => x.ProductId == productId);
    }
}