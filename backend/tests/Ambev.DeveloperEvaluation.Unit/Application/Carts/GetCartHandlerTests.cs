using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Carts;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

public class GetCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCartHandler> _logger;
    private GetCartHandler _handler;

    public GetCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetCartHandler>>();
        _handler = new GetCartHandler(_cartRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given a valid command, when Handle is called, then it should return a success result")]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var command = GetCartHandlerTestData.GenerateValidCommand();
        var cart = GetCartHandlerTestData.GenerateValidCart();
        var cartResult = GetCartResult.Create(cart.Id, cart.UserId, cart.Date, cart.Products);

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(cart);
        _mapper.Map<GetCartResult>(cart).Returns(cartResult);

        // Act
        var getCartResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<GetCartResult>(Arg.Is<Cart>(
            c => c.Id == cart.Id &&
                 c.UserId == cart.UserId &&
                 c.Date == cart.Date &&
                 c.Products == cart.Products));

        await _cartRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());

        getCartResult.Should().BeEquivalentTo(cartResult);
    }

    [Fact(DisplayName = "Given an valid command, when Handle is called, then it should throw a KeyNotFoundException")]
    public async Task Handle_ValidCommand_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = GetCartHandlerTestData.GenerateValidCommand();

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(null as Cart);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}