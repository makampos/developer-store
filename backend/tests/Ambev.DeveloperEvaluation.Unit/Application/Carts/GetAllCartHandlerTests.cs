using Ambev.DeveloperEvaluation.Application.Carts.GetAllCart;
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

public class GetAllCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllCartHandler> _logger;
    private readonly GetAllCartHandler _handler;

    public GetAllCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetAllCartHandler>>();
        _handler = new GetAllCartHandler(_cartRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid command When getting all carts Then returns success result")]
    public async Task GivenValidCommand_WhenGettingAllCarts_ThenReturnsSuccessResult()
    {
        // Given
        var listOfCarts = GetAllCartHandlerTestData.GetAllCarts(10);
        var request = GetAllCardCommand.Create(1, 10);
        var pagedResultOfCarts = PagedResult<Cart>.Create(listOfCarts[..10], 20, 10, 1);

        _cartRepository.GetAllCartsAsync(Arg.Any<int>(), Arg.Any<int>(),
                Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(pagedResultOfCarts);

        var getCartResult = pagedResultOfCarts.Items!.Select(x
            => new GetCartResult(x.Id, x.UserId, x.Date, x.Products)).ToList();

        var getAllCartsResult = PagedResult<GetCartResult>.Create(
            items: getCartResult,
            totalCount: pagedResultOfCarts.TotalCount,
            pageSize: pagedResultOfCarts.PageSize,
            currentPage: pagedResultOfCarts.CurrentPage);

        var resultMap = new GetAllCartResult(getAllCartsResult);

        _mapper.Map<GetAllCartResult>(pagedResultOfCarts).Returns(resultMap);

        // When
        var result = await _handler.Handle(request, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<GetAllCartResult>(Arg.Is<PagedResult<Cart>>(x =>
            x.Items!.Count == 10 &&
            x.TotalCount == 20 &&
            x.PageSize == 10 &&
            x.CurrentPage == 1));

        result.Should().BeEquivalentTo(resultMap);
    }
}