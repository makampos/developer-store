using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartHandler : IRequestHandler<GetCartCommand, GetCartResult>
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCartHandler> _logger;

    public GetCartHandler(ICartRepository repository, IMapper mapper, ILogger<GetCartHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetCartResult> Handle(GetCartCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {GetCartHandler} triggered to handle {GetCartCommand}",
            nameof(GetCartHandler), nameof(GetCartCommand));

        var cart = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (cart is null)
        {
            _logger.LogWarning("Cart with id {CartId} not found", command.Id);
            throw new KeyNotFoundException($"Cart with id {command.Id} not found");
        }

        return _mapper.Map<GetCartResult>(cart);
    }
}