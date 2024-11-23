using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository : ICartRepository
{
    private readonly DefaultContext _context;

    private DbSet<Cart> Set => _context.Set<Cart>();

    public CartRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await _context.Carts.AddAsync(cart, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    public async Task<Cart?> GetCartAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Carts.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteCartAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (cart is null)
        {
            return false;
        }

        _context.Carts.Remove(cart);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }
}