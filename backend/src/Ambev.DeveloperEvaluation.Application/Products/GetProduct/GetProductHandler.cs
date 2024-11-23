using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductHandler : IRequestHandler<GetProductCommand, GetProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductHandler> _logger;

    public GetProductHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductResult> Handle(GetProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {GetProductHandler} triggered to handle {GetProductCommand} with ProductId: {ProductId}",
            nameof(GetProductHandler), nameof(GetProductCommand), request.Id);

        var validator = new GetProductValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {GetProductCommand}", nameof(GetProductCommand));
            throw new ValidationException(validationResult.Errors);
        }

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            _logger.LogWarning("Product with Id: {ProductId} not found", request.Id);
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");
        }

        _logger.LogInformation("Product with Id: {ProductId} retrieved successfully", request.Id);

        return _mapper.Map<GetProductResult>(product);
    }
}