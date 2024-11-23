using Ambev.DeveloperEvaluation.Application.Products.GetAllProductsByCategory;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class GetAllProductsByCategoryHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductsByCategoryHandler> _logger;
    private readonly GetAllProductsByCategoryHandler _handler;

    public GetAllProductsByCategoryHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetAllProductsByCategoryHandler>>();
        _handler = new GetAllProductsByCategoryHandler(_productRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid command When getting all products by category Then returns success response")]
    public async Task GivenValidRequest_WhenGettingAllProductsByCategory_ThenReturnsSuccessResponse()
    {
        // Given
        var productsTestData = GetAllProductsByCategoryHandlerTestData.GetAllProductsByCategory(20);
        var category = productsTestData[0].Category;
        var request = GetAllProductsByCategoryCommand.Create(category, 1, 10);
        var (products, count) = (productsTestData[..20].Where(x => x.Category == category)
                .ToList(), productsTestData[..20].Count(x => x.Category == category));
        var pagedResult = PagedResult<Product>.Create(products, count, 10, 1);

        _productRepository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(),
            Arg.Any<string>(), Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(pagedResult);

        var getProductResult = pagedResult.Items!.Select(x
                => new GetProductResult( x.Id, x.Title, x.Price, x.Description, x.Category, x.Image, x.Rating)).ToList();

        var getAllProductsResult = PagedResult<GetProductResult>.Create(
            items: getProductResult,
            totalCount: pagedResult.TotalCount,
            pageSize: pagedResult.PageSize,
            currentPage: pagedResult.CurrentPage);

        var resultMap = new GetAllProductsByCategoryResult(getAllProductsResult);

        _mapper.Map<GetAllProductsByCategoryResult>(pagedResult).Returns(resultMap);

        // When
        var result = await _handler.Handle(request, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetAllProductsByCategoryResult>(Arg.Is<PagedResult<Product>>(x =>
            x.Items!.Count == count &&
            x.TotalCount == count &&
            x.PageSize == 10 &&
            x.CurrentPage == 1));

        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        result.Products.Items.Should().HaveCount(count);
        result.Products.PageSize.Should().Be(10);
        result.Products.TotalCount.Should().Be(count);
    }

    [Fact(DisplayName = "Given valid command When not found products by category Then returns empty response")]
    public async Task GivenValidRequest_WhenNotFoundProductsByCategory_ThenReturnsEmptyResponse()
    {
        // Given
        var category = "Not Found Category";
        var request = GetAllProductsByCategoryCommand.Create(category, 1, 10);
        var pagedResult = PagedResult<Product>.Create(new List<Product>(), 0, 10, 1);

        _productRepository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(),
            Arg.Any<string>(), Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(pagedResult);

        var getProductResult = pagedResult.Items!.Select(x
                => new GetProductResult( x.Id, x.Title, x.Price, x.Description, x.Category, x.Image, x.Rating)).ToList();

        var getAllProductsResult = PagedResult<GetProductResult>.Create(
            items: getProductResult,
            totalCount: pagedResult.TotalCount,
            pageSize: pagedResult.PageSize,
            currentPage: pagedResult.CurrentPage);

        var resultMap = new GetAllProductsByCategoryResult(getAllProductsResult);

        _mapper.Map<GetAllProductsByCategoryResult>(pagedResult).Returns(resultMap);

        // When
        var result = await _handler.Handle(request, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetAllProductsByCategoryResult>(Arg.Is<PagedResult<Product>>(x =>
            x.Items!.Count == 0 &&
            x.TotalCount == 0 &&
            x.PageSize == 10 &&
            x.CurrentPage == 1));

        result.Should().NotBeNull();
        result.Products.Should().NotBeNull();
        result.Products.Items.Should().HaveCount(0);
        result.Products.PageSize.Should().Be(10);
        result.Products.TotalCount.Should().Be(0);
    }
}