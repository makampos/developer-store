using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;

    public UpdateSaleHandler(ISaleRepository saleRepository, IProductRepository productRepository, IMapper mapper,
        ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {UpdateSaleHandler} triggered to handle {UpdateSaleCommand}",
            nameof(UpdateSaleHandler), nameof(UpdateSaleCommand));

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (sale is null)
        {
            _logger.LogWarning("Sale with id {SaleId} not found", command.Id);
            throw new KeyNotFoundException("Sale not found");
        }

        var productIds = command.SaleItems.Select(x => x.ProductId).ToList();
        var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);

        if (products.Count != productIds.Distinct().Count())
        {
            _logger.LogError("Some products were not found");
            throw new DomainException("Some products were not found");
        }

        // Create Instance of CartItem items
        var carItems = command.SaleItems.Select(saleItem =>
                CartItem.Create(saleItem.ProductId, saleItem.Quantity)).ToList();

        // Convert CartItem to CartItemDetails and calculate discount/amount
        var cartItemDetails = carItems
            .Select(cartItem => new CartItemDetails(cartItem)
                .ToCartItemDetails(products.First(product => product.Id == cartItem.ProductId))).ToList();

        sale.UpdateSale(cartItemDetails, products, command.Branch, command.UserId);

        await _saleRepository.UpdateAsync(sale, cancellationToken);
        _logger.LogInformation("Sale with id {SaleId} updated successfully", command.Id);

        // TODO: Publish event SaleUpdatedEvent

        return _mapper.Map<UpdateSaleResult>(sale);;
    }
}