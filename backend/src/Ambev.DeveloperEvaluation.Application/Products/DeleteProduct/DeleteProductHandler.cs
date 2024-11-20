using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(IProductRepository productRepository, ILogger<DeleteProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<DeleteProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {DeleteProductHandler} triggered to handle {DeleteProductCommand} with ProductId: {ProductId}",
            nameof(DeleteProductHandler), nameof(DeleteProductCommand), request.Id);

        var validator = new DeleteProductCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {DeleteProductCommand}", nameof(DeleteProductCommand));
            throw new ValidationException(validationResult.Errors);
        }

        var isSuccess = await _productRepository.DeleteAsync(request.Id, cancellationToken);
        if (!isSuccess)
        {
            _logger.LogWarning("Product with Id: {ProductId} not found", request.Id);
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");
        }

        _logger.LogInformation("Product with Id: {ProductId} deleted successfully", request.Id);

        return DeleteProductResponse.Create(isSuccess);
    }
}