using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using Guid = System.Guid;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
        _handler = new UpdateSaleHandler(_saleRepository, _productRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid command When updating sale Then returns success result")]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Given
        var createProductCommand = CreateProductHandlerTestData.GenerateValidCommand();
        var product = CreateProductHandlerTestData.GenerateValidProduct(createProductCommand, Guid.NewGuid());
        var products = new List<Product>() { product };

        var sale = GetSaleHandlerTestData.GenerateValidSale(product.Id, 1, 1);
        var updateSaleCommand = UpdateSaleHandlerTestData.GenerateValidCommand(sale.Id, [products.First().Id], 1);
        var saleToBeUpdated = GetSaleHandlerTestData.GenerateSaleWithSelectedSaleItems(updateSaleCommand.SaleItems);
        var carItems = saleToBeUpdated.SaleItems.Select(saleItem =>
                CartItem.Create(saleItem.ProductId, saleItem.Quantity)).ToList();
        var cartItemDetails = carItems
            .Select(cartItem => new CartItemDetails(cartItem)
                .ToCartItemDetails(products.First(p => p.Id == cartItem.ProductId))).ToList();

        saleToBeUpdated.UpdateSale(cartItemDetails, products, updateSaleCommand.Branch, updateSaleCommand.UserId);
        var saleUpdatedResult = UpdateSaleHandlerTestData.GenerateValidResult(saleToBeUpdated);

        _productRepository.GetByIdsAsync(Arg.Any<List<Guid>>(), Arg.Any<CancellationToken>()).Returns(products);
        _saleRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(saleToBeUpdated);

        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(saleUpdatedResult);

        // When
        var result = await _handler.Handle(updateSaleCommand, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).GetByIdAsync(updateSaleCommand.Id, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).GetByIdsAsync(Arg.Any<List<Guid>>(), Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());

        result.Should().BeEquivalentTo(saleUpdatedResult);
    }

    [Fact(DisplayName = " Given invalid command When updating sale Then throws KeyNotFoundException")]
    public async Task Handle_InvalidCommand_KeyNotFoundException()
    {
        // Given
        var updateSaleCommand = UpdateSaleHandlerTestData.GenerateInvalidCommand();

        // When
        Func<Task> act = async () => await _handler.Handle(updateSaleCommand, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // test for DomainException
    [Fact(DisplayName = "Given invalid product When updating sale Then throws DomainException")]
    public async Task Handle_InvalidProduct_DomainException()
    {
        // Given
        var createProductCommand = CreateProductHandlerTestData.GenerateValidCommand();
        var product = CreateProductHandlerTestData.GenerateValidProduct(createProductCommand, Guid.NewGuid());
        var products = new List<Product>() { product };

        var sale = GetSaleHandlerTestData.GenerateValidSale(product.Id, 1, 1);
        var updateSaleCommand = UpdateSaleHandlerTestData.GenerateValidCommand(sale.Id, [products.First().Id], 1);

        _productRepository.GetByIdsAsync(Arg.Any<List<Guid>>(), Arg.Any<CancellationToken>()).Returns(new List<Product>
            ());
        _saleRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        Func<Task> act = async () => await _handler.Handle(updateSaleCommand, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<DomainException>();
    }
}