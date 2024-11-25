using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;
    private DbSet<Sale> Set => _context.Set<Sale>();

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

     public async Task<PagedResult<Sale>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? order = null,
        CancellationToken cancellationToken = default)
    {
        var query = SetAsNoTracking;

        if (!string.IsNullOrEmpty(order))
        {
            var orderedQueryable = query as IOrderedQueryable<Sale>;

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

        return PagedResult<Sale>.Create(items, totalCount, pageSize, pageNumber);
    }

    public async Task<Sale?> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    private IQueryable<Sale> SetAsNoTracking
    {
        get
        {
            var query = Set.AsNoTracking();

            return query;
        }
    }
}