using Ambev.DeveloperEvaluation.Application.Products.Categories;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.Categories;

public class GetAllProductCategoriesHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetAllProductCategoriesHandler> _logger;
    private readonly GetAllProductCategoriesHandler _handler;

    public GetAllProductCategoriesHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _logger = Substitute.For<ILogger<GetAllProductCategoriesHandler>>();
        _handler = new GetAllProductCategoriesHandler(_productRepository, _logger);
    }

    [Fact(DisplayName = "Given a command, When getting product categories, Then returns success response")]
    public async Task GivenValidCommand_WhenGettingCategories_ThenReturnsSuccessResponse()
    {
        // Given
        var faker = new Faker();
        var categories = faker.Commerce.Categories(3);

        _productRepository.GetCategoriesAsync(Arg.Any<CancellationToken>()).Returns(categories);

        // When
        var result = await _handler.Handle(new GetAllProductCategoriesCommand(), CancellationToken.None);

        // Then
        result.ProductCategories.Should().BeEquivalentTo(categories);
    }
}