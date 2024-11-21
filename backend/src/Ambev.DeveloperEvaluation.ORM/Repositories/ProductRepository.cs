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
        CancellationToken cancellationToken = default)
    {
        var totalCount = await SetAsNoTracking.CountAsync(cancellationToken);
        var items = await SetAsNoTracking
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