using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(ISaleRepository saleRepository, IProductRepository productRepository, IMapper mapper, ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {CreateSaleHandler} triggered to handle {CreateSalesCommand}",
            nameof(CreateSaleHandler), nameof(CreateSaleCommand));

        var productIds = command.CartItems.Select(x => x.ProductId).ToList();
        var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);

        if (products.Count != productIds.Distinct().Count())
        {
            _logger.LogError("Some products were not found");
            throw new Exception("Some products were not found");
        }

        var cartItemDetails = command.CartItems
            .Select(cartItem => new CartItemDetails(cartItem)
                .ToCartItemDetails(products.First(product => product.Id == cartItem.ProductId))).ToList();

        var sale = new Sale();

        sale.BuildSale(
             cartItems: cartItemDetails,
             products: products,
             branch: command.Branch,
             userId: command.UserId);

        var saleCreated = await _saleRepository.CreateAsync(sale, cancellationToken);
        var map = _mapper.Map<CreateSaleResult>(saleCreated);

        return map;
    }
}