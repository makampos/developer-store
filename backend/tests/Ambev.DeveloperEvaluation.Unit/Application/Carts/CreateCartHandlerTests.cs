using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Carts;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

public class CreateCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCartHandler> _logger;
    private CreateCartHandler _handler;

    public CreateCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateCartHandler>>();
        _handler = new CreateCartHandler(_cartRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Handle_ValidCommand_ReturnsSuccessResult")]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        var cart = Cart.Create(command.UserId, DateTime.Now, command.CartItems);
        var cartResult = CreateCartResult.Create(
            id: Guid.NewGuid(),
            userId: cart.UserId,
            date: cart.Date,
            cartItems: cart.Products);

        cart.Id = cartResult.Id;

        _mapper.Map<Cart>(command).Returns(cart);
        _mapper.Map<CreateCartResult>(cart).Returns(cartResult);

        _cartRepository.CreateAsync(cart, Arg.Any<CancellationToken>()).Returns(cart);

        // Act
        var createCartResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<Cart>(Arg.Is<CreateCartCommand>(
            c => c.UserId == command.UserId &&
                 c.Date == command.Date &&
                 c.CartItems == command.CartItems));

        await _cartRepository.Received(1).CreateAsync(cart, Arg.Any<CancellationToken>());

        createCartResult.Should().BeEquivalentTo(cartResult);
    }
}