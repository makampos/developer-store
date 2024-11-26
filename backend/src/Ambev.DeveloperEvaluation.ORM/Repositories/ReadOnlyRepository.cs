using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ReadOnlyRepository<TEntity>(
    DefaultContext defaultContext
) : RepositoryProperties<TEntity>(defaultContext), IReadOnlyRepository<TEntity> where TEntity : BaseEntity
{
    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await SetAsTracking.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedResult<TEntity>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? order = null,
        string? category = null,
        CancellationToken cancellationToken = default
        )
    {
        var totalCount = await SetAsNoTracking.CountAsync(cancellationToken);
        var items = await SetAsNoTracking
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return  PagedResult<TEntity>.Create(items, totalCount, pageNumber, pageSize);
    }
}