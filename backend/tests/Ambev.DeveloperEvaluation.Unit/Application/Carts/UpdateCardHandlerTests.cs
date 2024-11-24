using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Carts;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

public class UpdateCardHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateCardHandler> _logger;
    private readonly UpdateCardHandler _handler;

    public UpdateCardHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<UpdateCardHandler>>();
        _handler = new UpdateCardHandler(_cartRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid command When Handle Then return updated cart")]
    public async Task GivenValidCommand_WhenHandle_ThenReturnUpdatedCart()
    {
        // Given
        var command = UpdateHandlerTestData.GenerateValidCommand();
        var cart = UpdateHandlerTestData.GenerateCart();

        var cartResult = UpdateCartResult.Create(
            id: cart.Id,
            userId: cart.UserId,
            date: cart.Date,
            cartItems: cart.Products);

        _cartRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(cart);
        _cartRepository.UpdateAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>()).Returns(cart);
        _mapper.Map<UpdateCartResult>(Arg.Any<Cart>()).Returns(cartResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
       _mapper.Received(1).Map<UpdateCartResult>(Arg.Is<Cart>(
            c => c.Id == cart.Id &&
                 c.Products == cart.Products));

        await _cartRepository.Received(1).UpdateAsync(cart, Arg.Any<CancellationToken>());

        result.Should().BeEquivalentTo(cartResult);
    }

    [Fact(DisplayName = "Given invalid command When Handle Then throws validation exception")]
    public async Task GivenInvalidCommand_WhenHandle_ThenThrowsValidationException()
    {
        // Given
        var command = new UpdateCartCommand();

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given cart not found When Handle Then throws validation exception")]
    public async Task GivenCartNotFound_WhenHandle_ThenThrowsValidationException()
    {
        // Given
        var command = UpdateHandlerTestData.GenerateValidCommand();

        _cartRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(null as Cart);

        // When
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}