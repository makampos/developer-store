using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _handler = new CreateSaleHandler(_saleRepository, _productRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid command When creating sale Then returns success result")]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Given
        var createProductCommand1 = CreateProductHandlerTestData.GenerateValidCommand();
        var product1 = CreateProductHandlerTestData.GenerateValidProduct(createProductCommand1, Guid.NewGuid());
        var createSaleCommand1 = CreateSaleHandlerTestData.GenerateValidCommandWithValidCartItems(product1.Id, 1);

        var createProductCommand2 = CreateProductHandlerTestData.GenerateValidCommand();
        var product2 = CreateProductHandlerTestData.GenerateValidProduct(createProductCommand2, Guid.NewGuid());
        var createSaleCommand2 = CreateSaleHandlerTestData.GenerateValidCommandWithValidCartItems(product2.Id, 4);

        var mergedList = createSaleCommand1.CartItems.Concat(createSaleCommand2.CartItems).ToList();

        var mergedCommand = createSaleCommand2 with { CartItems = mergedList };
        var mergedProducts = new List<Product>() { product1, product2 };


        var saleId = 11;
        var saleResult = new Sale() { Id = saleId }; // omitted other properties for brevity
        var createSaleResult = CreateSaleResult.CreateSaleResultFromSale(saleResult);

        _productRepository.GetByIdsAsync(Arg.Any<List<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(mergedProducts);

        _mapper.Map<CreateSaleResult>(saleResult).Returns(createSaleResult);

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(saleResult);

        // When
        var result = await _handler.Handle(mergedCommand, CancellationToken.None);

        // Then
        result.Should().BeEquivalentTo(createSaleResult);
    }
}