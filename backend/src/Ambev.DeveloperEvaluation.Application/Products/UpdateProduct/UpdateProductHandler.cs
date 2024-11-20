using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(IProductRepository productRepository, IMapper mapper, ILogger<UpdateProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {UpdateProductHandler} triggered to handle {UpdateProductCommand}",
            nameof(UpdateProductHandler), nameof(UpdateProductCommand));

        var validator = new UpdateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {UpdateProductCommand}", nameof(UpdateProductCommand));
            throw new ValidationException(validationResult.Errors);
        }

        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

        if (product is null)
        {
            _logger.LogWarning("Product with Id: {ProductId} not found", command.Id);
            throw new ValidationException(new List<ValidationFailure>
            {
                new("Id", $"Product with Id: {command.Id} not found")
            });
        }

        product.Update(
            title: command.Title,
            price: command.Price,
            description: command.Description,
            category: command.Category,
            image: command.Image,
            rating: command.Rating);

        var updatedProduct = await _productRepository.UpdateAsync(product, cancellationToken);

        _logger.LogInformation("Product with Id: {ProductId} updated successfully", updatedProduct.Id);

        return _mapper.Map<UpdateProductResult>(updatedProduct);
    }
}