using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleHandler> _logger;

    public CancelSaleHandler(ISaleRepository saleRepository, ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {CancelSaleHandler} triggered to handle {CancelSaleCommand}",
            nameof(CancelSaleHandler), command);

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);

        if (sale is null)
        {
            _logger.LogWarning("Sale with id {SaleId} not found", command.SaleId);
            throw new KeyNotFoundException("Sale not found");
        }

        sale.CancelSale();

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // TODO: Publish event SaleCanceledEvent

        return CancelSaleResult.Create("Sale canceled successfully");
    }
}