using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;

    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<GetSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetSaleResult> Handle(GetSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {GetSaleHandler} triggered to handle {GetSaleCommand}",
            nameof(GetSaleHandler), nameof(GetSaleCommand));

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (sale is null)
        {
            _logger.LogError("Sale not found");
            throw new KeyNotFoundException("Sale not found");
        }

        return _mapper.Map<GetSaleResult>(sale);
    }
}