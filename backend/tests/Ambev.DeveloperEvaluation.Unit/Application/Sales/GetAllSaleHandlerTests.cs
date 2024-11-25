using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class GetAllSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllSaleHandler> _logger;
    private readonly GetAllSaleHandler _handler;

    public GetAllSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetAllSaleHandler>>();
        _handler = new GetAllSaleHandler(_saleRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid command When getting all sales Then returns success result")]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Given
        var request = GetAllSaleCommand.Create(1, 10, "SaleDate");
        var listOfSales = GetAllSaleHandlerTestData.GetAllSales(10);
        var pagedResultOfSales = PagedResult<Sale>.Create(listOfSales[..3], 3, 10, 1);

        _saleRepository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(pagedResultOfSales);

        var getSaleResult = pagedResultOfSales.Items!.Select(x
            => new GetSaleResult(x.Id, x.UserId, x.SaleDate, x.SaleItems, x.TotalSaleAmount, x.TotalSaleDiscount,
                x.Branch, x.IsCanceled)).ToList();

        var getAllSalesResult = PagedResult<GetSaleResult>.Create(
            items: getSaleResult,
            totalCount: pagedResultOfSales.TotalCount,
            pageSize: pagedResultOfSales.PageSize,
            currentPage: pagedResultOfSales.CurrentPage);

        var resultMap = new GetAllSaleResult(getAllSalesResult);

        _mapper.Map<GetAllSaleResult>(Arg.Any<PagedResult<Sale>>()).Returns(resultMap);

        // When
        var result = await _handler.Handle(request, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetAllSaleResult>(Arg.Is<PagedResult<Sale>>(x =>
            x.Items!.Count == 3 &&
            x.TotalCount == 3 &&
            x.PageSize == 10 &&
            x.CurrentPage == 1 &&
            x.TotalPages == 1));

        result.Should().NotBeNull();
        result.Sales.Should().NotBeNull();
        result.Sales.Items.Should().NotBeEmpty();
        result.Sales.Items.Should().HaveCount(3);
        result.Sales.PageSize.Should().Be(10);
        result.Sales.TotalCount.Should().Be(3);
        result.Sales.CurrentPage.Should().Be(1);
        result.Sales.TotalPages.Should().Be(1);
        result.Sales.HasPreviousPage.Should().BeFalse();
        result.Sales.HasNextPage.Should().BeFalse();
    }
}