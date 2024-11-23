using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.Categories;
public class GetAllProductCategoriesHandler : IRequestHandler<GetAllProductCategoriesCommand, GetAllProductCategoriesResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetAllProductCategoriesHandler> _logger;

    public GetAllProductCategoriesHandler(IProductRepository productRepository, ILogger<GetAllProductCategoriesHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<GetAllProductCategoriesResult> Handle(GetAllProductCategoriesCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {GetCategoriesHandler} triggered to handle  {GetAllProductCategoriesCommand}",
            nameof(GetAllProductCategoriesHandler), nameof(GetAllProductCategoriesCommand));

        var productCategories = await _productRepository.GetCategoriesAsync(cancellationToken);
        return GetAllProductCategoriesResult.Create(productCategories);
    }
}