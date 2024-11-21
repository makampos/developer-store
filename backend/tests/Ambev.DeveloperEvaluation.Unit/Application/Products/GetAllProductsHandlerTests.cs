using Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class GetAllProductsHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductsHandler> _logger;
    private readonly GetAllProductsHandler _handler;

    public GetAllProductsHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetAllProductsHandler>>();
        _handler = new GetAllProductsHandler(_productRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid request When getting all products Then returns success response")]
    public async Task GivenValidRequest_WhenGettingAllProducts_ThenReturnsSuccessResponse()
    {
        // Given
        var listOfProducts = GetAllProductsHandlerTestData.GetAllProducts(20);
        var request = GetAllProductsCommand.Create(1, 10);
        var pagedResult = PagedResult<Product>.Create(listOfProducts[..10], 20, 10, 1);

        _productRepository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(),
            Arg.Any<CancellationToken>()).Returns(pagedResult);

        _mapper.Map<GetAllProductsResult>(pagedResult).Returns(new GetAllProductsResult(pagedResult));

        // When
        var result = await _handler.Handle(request, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetAllProductsResult>(pagedResult);

        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        result.Products.Items.Should().NotBeEmpty();
        result.Products.Items.Should().HaveCount(10);
        result.Products.PageSize.Should().Be(10);
        result.Products.TotalCount.Should().Be(20);
        result.Products.CurrentPage.Should().Be(1);
        result.Products.TotalPages.Should().Be(2);
        result.Products.HasPreviousPage.Should().BeFalse();
        result.Products.HasNextPage.Should().BeTrue();
    }

    [Fact(DisplayName = "Given valid request When getting all products Then returns empty list")]
    public async Task GivenValidRequest_WhenGettingAllProducts_ThenReturnsEmptyList()
    {
        // Given
        var request = GetAllProductsCommand.Create(1, 10);
        var listOfProducts = GetAllProductsHandlerTestData.GetAllProducts(0);
        var pagedResult = PagedResult<Product>.Create(listOfProducts, 0, 10, 1);

        _productRepository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(),
            Arg.Any<CancellationToken>()).Returns(pagedResult);

        _mapper.Map<GetAllProductsResult>(pagedResult).Returns(new GetAllProductsResult(pagedResult));

        // When
        var result = await _handler.Handle(request, CancellationToken.None);

        // Then

        _mapper.Received(1).Map<GetAllProductsResult>(pagedResult);

        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        result.Products.Items.Should().BeEmpty();
        result.Products.PageSize.Should().Be(10);
        result.Products.TotalCount.Should().Be(0);
        result.Products.CurrentPage.Should().Be(1);
        result.Products.TotalPages.Should().Be(0);
        result.Products.HasPreviousPage.Should().BeFalse();
        result.Products.HasNextPage.Should().BeFalse();
    }
}