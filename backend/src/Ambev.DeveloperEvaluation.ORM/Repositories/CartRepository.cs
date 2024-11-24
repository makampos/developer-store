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

    public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
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

     public async Task<PagedResult<Cart>> GetAllCartsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? order = null,
        CancellationToken cancellationToken = default)
    {
        var query = SetAsNoTracking;

        if (!string.IsNullOrEmpty(order))
        {
            var orderedQueryable = query as IOrderedQueryable<Cart>;

            var orderByFields = order.Split([','], StringSplitOptions.RemoveEmptyEntries);

            var isFirstOrdering = true;

            foreach (var field in orderByFields)
            {
                var trimmedField = field.Trim();

                if (trimmedField.EndsWith(" desc", StringComparison.OrdinalIgnoreCase))
                {
                    trimmedField = trimmedField.Substring(0, trimmedField.Length - 5).Trim();
                    trimmedField = char.ToUpper(trimmedField[0]) + trimmedField.Substring(1);

                    if (isFirstOrdering)
                    {
                        orderedQueryable = orderedQueryable.OrderByDescending(p => EF.Property<object>(p, trimmedField));
                        isFirstOrdering = false;
                    }
                    else
                    {
                        orderedQueryable = orderedQueryable.ThenByDescending(p => EF.Property<object>(p, trimmedField));
                    }
                }

                else if (trimmedField.EndsWith(" asc", StringComparison.OrdinalIgnoreCase))
                {
                    trimmedField = trimmedField.Substring(0, trimmedField.Length - 4).Trim();
                    trimmedField = char.ToUpper(trimmedField[0]) + trimmedField.Substring(1);

                    if (isFirstOrdering)
                    {
                        orderedQueryable = orderedQueryable.OrderBy(p => EF.Property<object>(p, trimmedField));
                        isFirstOrdering = false;
                    }
                    else
                    {
                        orderedQueryable = orderedQueryable.ThenBy(p => EF.Property<object>(p, trimmedField));

                    }
                }
                else
                {
                    trimmedField = char.ToUpper(trimmedField[0]) + trimmedField.Substring(1);

                    if (isFirstOrdering)
                    {
                        orderedQueryable = orderedQueryable.OrderBy(p => EF.Property<object>(p, trimmedField));
                        isFirstOrdering = false;
                    }
                    else
                    {
                        orderedQueryable = orderedQueryable.ThenBy(p => EF.Property<object>(p, trimmedField));
                    }
                }
            }

            query = orderedQueryable;
        }

        var totalCount = await query!.CountAsync(cancellationToken);
        var items = await query!
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return PagedResult<Cart>.Create(items, totalCount, pageSize, pageNumber);
    }

    public async Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    private IQueryable<Cart> SetAsNoTracking
    {
        get
        {
            var query = Set.AsNoTracking();

            return query;
        }
    }
}