using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using Ambev.DeveloperEvaluation.Unit.Domain.Products;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class UpdateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductHandler> _logger;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<UpdateProductHandler>>();
        _handler = new UpdateProductHandler(_productRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given invalid product data When updating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new UpdateProductCommand();

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given product not found When updating product Then throws validation exception")]
    public async Task Handle_ProductNotFound_ThrowsValidationException()
    {
        // Given
        var command = UpdateProductHandlerTestData.GenerateValidCommand();

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(null as Product);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given valid product data When updating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var commandToUpdateProduct = UpdateProductHandlerTestData.GenerateValidCommand();
        var commandToCreateProduct = CreateProductHandlerTestData.GenerateValidCommand();
        var product = Product.Create(commandToCreateProduct.Title, commandToCreateProduct.Price,
            commandToCreateProduct.Description, commandToCreateProduct.Category, commandToCreateProduct.Image,
            commandToCreateProduct.Rating);

        product.Id = commandToUpdateProduct.Id;

        var updateProductResult = UpdateProductResult.Create(product.Id, commandToUpdateProduct.Title, commandToUpdateProduct
            .Price,
            commandToUpdateProduct.Description, commandToUpdateProduct.Category, commandToUpdateProduct.Image,
            commandToUpdateProduct.Rating);

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<UpdateProductResult>(product).Returns(updateProductResult);
        _productRepository.UpdateAsync(product, Arg.Any<CancellationToken>()).Returns(product);

        // When
        var result = await _handler.Handle(commandToUpdateProduct, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<UpdateProductResult>(Arg.Is<Product>(p =>
            p.Id == product.Id &&
            p.Title == product.Title &&
            p.Price == product.Price &&
            p.Description == product.Description &&
            p.Category == product.Category &&
            p.Image == product.Image &&
            p.Rating == product.Rating
        ));

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updateProductResult);
    }
}