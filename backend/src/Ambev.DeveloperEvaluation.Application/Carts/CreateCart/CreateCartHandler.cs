using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCartHandler> _logger;

    public CreateCartHandler(ICartRepository cartRepository, IMapper mapper, ILogger<CreateCartHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {CreateCartHandler} triggered to handle {CreateCartCommand} for {UserId}",
            nameof(CreateCartHandler), nameof(CreateCartCommand), command.UserId);

        //TODO: Implement Validator

        //TODO: Add Validation to ensure that the products exist before attempting to create the cart

        var cart = _mapper.Map<Cart>(command);
        var cartCreated = await _cartRepository.CreateAsync(cart, cancellationToken);
        var result = _mapper.Map<CreateCartResult>(cartCreated);
        return result;
    }
}