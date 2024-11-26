using Ambev.DeveloperEvaluation.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class RepositoryProperties<TEntity>(
    DefaultContext defaultContext
) where TEntity : BaseEntity
{
    protected readonly DefaultContext ApplicationDbContext = defaultContext;

    protected DbSet<TEntity> Set => ApplicationDbContext.Set<TEntity>();

    protected IQueryable<TEntity> SetAsTracking
    {
        get
        {
            var query = Set.AsTracking();

            return query;
        }
    }

    protected IQueryable<TEntity> SetAsNoTracking
    {
        get
        {
            var query = Set.AsNoTracking();

            return query;
        }
    }
}