using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Product>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);
}