using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCardHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateCardHandler> _logger;

    public UpdateCardHandler(ICartRepository cartRepository, IMapper mapper, ILogger<UpdateCardHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {UpdateCardHandler} triggered to handle {UpdateCartCommand}", nameof
            (UpdateCardHandler), nameof(UpdateCartCommand));

        var validator = new UpdateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {UpdateCartCommand}", nameof(UpdateCartCommand));
            throw new ValidationException(validationResult.Errors);
        }

        var cart = await _cartRepository.GetByIdAsync(command.Id, cancellationToken);

        if (cart is null)
        {
            _logger.LogWarning("Cart not found for {UpdateCartCommand}", nameof(UpdateCartCommand));
            throw new ValidationException("Cart not found");
        }

        cart.Update(command.CartItems);

        var updatedCart = await _cartRepository.UpdateAsync(cart, cancellationToken);

        var mapper = _mapper.Map<UpdateCartResult>(updatedCart);

        return mapper;
    }
}