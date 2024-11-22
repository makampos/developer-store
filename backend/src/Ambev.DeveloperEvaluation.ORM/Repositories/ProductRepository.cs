using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;

    private DbSet<Product> Set => _context.Set<Product>();

    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return false;
        }

        _context.Products.Remove(product);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

   public async Task<PagedResult<Product>> GetAllAsync(
    int pageNumber = 1,
    int pageSize = 10,
    string? order = null,
    CancellationToken cancellationToken = default)
{
    var query = SetAsNoTracking;

    if (!string.IsNullOrEmpty(order))
    {
        var orderedQueryable = query as IOrderedQueryable<Product>;

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

    return PagedResult<Product>.Create(items, totalCount, pageSize, pageNumber);
}

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    private IQueryable<Product> SetAsNoTracking
    {
        get
        {
            var query = Set.AsNoTracking();

            return query;
        }
    }
}