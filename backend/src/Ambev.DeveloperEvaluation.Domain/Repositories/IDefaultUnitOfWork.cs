namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IDefaultUnitOfWork : IUnitOfWork
{
    IProductRepository ProductRepository { get; init; }
}