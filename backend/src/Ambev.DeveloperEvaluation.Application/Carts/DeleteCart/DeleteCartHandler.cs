using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResult>
{
    private ICartRepository _cartRepository;
    private readonly ILogger<DeleteCartHandler> _logger;

    public DeleteCartHandler(ICartRepository cartRepository, ILogger<DeleteCartHandler> logger)
    {
        _cartRepository = cartRepository;
        _logger = logger;
    }

    public async Task<DeleteCartResult> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {DeleteCartHandler} triggered to handle {DeleteCartCommand}",
            nameof(DeleteCartHandler), nameof(DeleteCartCommand));

        var validator = new DeleteCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {DeleteCartCommand}", nameof(DeleteCartCommand));
            throw new ValidationException(validationResult.Errors);
        }

        var isSuccess = await _cartRepository.DeleteCartAsync(command.Id, cancellationToken);

        if (!isSuccess)
        {
            _logger.LogWarning("Cart with Id: {CartId} not found", command.Id);
            throw new KeyNotFoundException($"Cart with ID {command.Id} not found");
        }

        _logger.LogInformation("Cart with Id: {CartId} deleted successfully", command.Id);
        return DeleteCartResult.Create(isSuccess);
    }
}