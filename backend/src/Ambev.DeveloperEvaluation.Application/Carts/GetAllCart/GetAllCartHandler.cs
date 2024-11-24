using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetAllCart;

public class GetAllCartHandler : IRequestHandler<GetAllCardCommand, GetAllCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllCartHandler> _logger;

    public GetAllCartHandler(ICartRepository cartRepository, IMapper mapper, ILogger<GetAllCartHandler> logger)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetAllCartResult> Handle(GetAllCardCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {GetAllCartHandler} triggered to handle {GetAllCardCommand}",
            nameof(GetAllCartHandler), nameof(GetAllCardCommand));

        // TODO: Implement Validator

        var pagedResultOfCarts = await _cartRepository.GetAllCartsAsync(command.PageNumber, command.PageSize,
            command.Order, cancellationToken);

        _logger.LogInformation("Carts retrieved successfully");

        var mappedCarts = _mapper.Map<GetAllCartResult>(pagedResultOfCarts);

        return mappedCarts;
    }
}