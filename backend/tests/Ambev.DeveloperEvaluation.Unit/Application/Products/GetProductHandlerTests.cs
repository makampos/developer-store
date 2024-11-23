using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductHandler> _logger;
    private readonly GetProductHandler _handler;

    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetProductHandler>>();
        _handler = new GetProductHandler(_productRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given invalid request When getting product Then throws validation exception")]
    public async Task GivenInvalidRequest_WhenGettingProduct_ThenThrowsValidationException()
    {
        // Given
        var request = GetProductCommand.Create(Guid.Empty);

        // When
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given product not found When getting product Then throws key not found exception")]
    public async Task GivenProductNotFound_WhenGettingProduct_ThenThrowsKeyNotFoundException()
    {
        // Given
        var id = Guid.NewGuid();
        var request = GetProductCommand.Create(id);

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(null as Product);

        // When
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GivenValidRequest_WhenGettingProduct_ThenReturnsSuccessResponse()
    {
        // Given
        var id = Guid.NewGuid();
        var request = GetProductCommand.Create(id);
        var createProductCommand = CreateProductHandlerTestData.GenerateValidCommand();

        var product = Product.Create(
            createProductCommand.Title,
            createProductCommand.Price,
            createProductCommand.Description,
            createProductCommand.Category,
            createProductCommand.Image,
            createProductCommand.Rating);

        product.Id = id;

        var result = GetProductResult.Create(
            product.Id,
            product.Title,
            product.Price,
            product.Description,
            product.Category,
            product.Image,
            product.Rating);

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(result);

        // When
        var response = await _handler.Handle(request, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetProductResult>(Arg.Is<Product>(p =>
            p.Id == result.Id &&
            p.Title == result.Title &&
            p.Price == result.Price &&
            p.Description == result.Description &&
            p.Category == result.Category &&
            p.Image == result.Image &&
            p.Rating == result.Rating
        ));

        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(result);
    }
}