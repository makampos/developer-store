using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

public class DeleteCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<DeleteCartHandler> _logger;
    private DeleteCartHandler _handler;

    public DeleteCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _logger = Substitute.For<ILogger<DeleteCartHandler>>();
        _handler = new DeleteCartHandler(_cartRepository, _logger);
    }

    [Fact(DisplayName = "Given a valid command, When Handle is called, Then should delete the cart")]
    public async Task GivenAValidCommand_WhenHandleIsCalled_ThenShouldDeleteTheCart()
    {
        // Given
        var command = DeleteCartCommand.Create(Guid.NewGuid());
        _cartRepository.DeleteCartAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "Given an valid command, When Handle is called, Then should throw a KeyNotFoundException")]
    public async Task Handle_InvalidCommand_ThrowsKeyNotFoundException()
    {
        // Given
        var command = DeleteCartCommand.Create(Guid.NewGuid());
        _cartRepository.DeleteCartAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact(DisplayName = "Given an invalid command, When Handle is called, Then should throw a ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var command = DeleteCartCommand.Create(Guid.Empty);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}