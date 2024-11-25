using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _logger = Substitute.For<ILogger<CancelSaleHandler>>();
        _handler = new CancelSaleHandler(_saleRepository, _logger);
    }

    [Fact(DisplayName = "Given valid command when cancel sale then return success message")]
    public async Task GivenValidCommand_WhenCancelSale_ThenReturnSuccessMessage()
    {
        // Given
        var command = CancelSaleCommand.Create(99);
        var createSaleCommand = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = GetSaleHandlerTestData.GenerateValidSale(createSaleCommand.CartItems[0].ProductId, 1,1);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
        result.Should().NotBeNull();
        result.Message.Should().Be("Sale canceled successfully");
    }

    [Fact(DisplayName = "Given invalid command when cancel sale then throw KeyNotFoundException")]
    public async Task GivenInvalidCommand_WhenCancelSale_ThenThrowKeyNotFoundException()
    {
        // Given
        var command = CancelSaleCommand.Create(99);

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).ReturnsNull();

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Sale not found");
        await _saleRepository.Received(1).GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>());
    }
}