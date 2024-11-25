using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
    Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<Sale>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? order = null,
        CancellationToken cancellationToken = default);
    Task<Sale?> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
}