using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class DefaultUnitOfWork(
    DefaultContext defaultContext,
    IProductRepository productRepository)  : UnitOfWork(defaultContext), IDefaultUnitOfWork
{
    public IProductRepository ProductRepository { get; init; } = productRepository;
}