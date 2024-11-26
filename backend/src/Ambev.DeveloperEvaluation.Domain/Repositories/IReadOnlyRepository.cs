using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IReadOnlyRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<TEntity>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? order = null,
        string? category = null,
        CancellationToken cancellationToken = default);
}