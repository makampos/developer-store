using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }
    public List<CartItem> Products { get; private set; } = [];

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

    public void Update(List<CartItem> products)
    {
        Products = products;
    }
}