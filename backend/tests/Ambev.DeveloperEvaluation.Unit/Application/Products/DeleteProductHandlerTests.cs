using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductHandler> _logger;
    private readonly DeleteProductHandler _handler;

    public DeleteProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _logger = Substitute.For<ILogger<DeleteProductHandler>>();
        _handler = new DeleteProductHandler(_productRepository, _logger);
    }

    [Fact(DisplayName = "Given invalid product data When deleting product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var id = Guid.Empty;
        var command = DeleteProductCommand.Create(id);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given product not found When deleting product Then throws key not found exception")]
    public async Task Handle_ProductNotFound_ThrowsKeyNotFoundException()
    {
        // Given
        var id = Guid.NewGuid();
        var command = DeleteProductCommand.Create(id);

        _productRepository.DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(false);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact(DisplayName = "Given valid product data When deleting product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var id = Guid.NewGuid();
        var command = DeleteProductCommand.Create(id);

        _productRepository.DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(true);

        // When
        var response = await _handler.Handle(command, CancellationToken.None);

        // Then
        response.Success.Should().BeTrue();
    }
}