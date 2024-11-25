using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
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

public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetSaleHandler>>();
        _handler = new GetSaleHandler(_saleRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid command When getting sale Then returns success result")]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Given
        var createProductCommand = CreateProductHandlerTestData.GenerateValidCommand();
        var product = CreateProductHandlerTestData.GenerateValidProduct(createProductCommand, Guid.NewGuid());
        var sale = GetSaleHandlerTestData.GenerateValidSale(product.Id, 1, 1);
        var getSaleCommand = new GetSaleCommand(sale.Id);

        var saleId = sale.Id;

        var getSaleResult = GetSaleResult.CreateGetSaleResultFromSale(sale);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(getSaleResult);

        // When
        var result = await _handler.Handle(getSaleCommand, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetSaleResult>(Arg.Is<Sale>(s =>
            s.Id == sale.Id &&
            s.UserId == sale.UserId &&
            s.SaleDate == sale.SaleDate &&
            s.SaleItems == sale.SaleItems &&
            s.TotalSaleAmount == sale.TotalSaleAmount &&
            s.TotalSaleDiscount == sale.TotalSaleDiscount &&
            s.Branch == sale.Branch &&
            s.IsCanceled == sale.IsCanceled));

        await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());

        result.Should().BeEquivalentTo(getSaleResult);
    }

    [Fact(DisplayName = "Given invalid command When getting sale Then throws exception")]
    public async Task Handle_InvalidCommand_ThrowsException()
    {
        // Given
        var getSaleCommand = new GetSaleCommand(22);

        _saleRepository.GetByIdAsync(getSaleCommand.Id, Arg.Any<CancellationToken>()).Returns((null as Sale));

        // When
        Func<Task> act = async () => await _handler.Handle(getSaleCommand, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Sale not found");
    }
}