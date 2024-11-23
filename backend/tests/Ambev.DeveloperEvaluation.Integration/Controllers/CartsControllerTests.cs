using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Integration.Configurations;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Carts.CreateCart;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Controllers;

public class CartsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public CartsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.InitializeAsync().GetAwaiter().GetResult();
        _client = _factory.CreateClient();
    }

    [Fact(DisplayName = "Given a valid request, When creating a cart, Then it should return Created StatusCode " +
                        "and a cart response")]
    public async Task  CreateCart_WithValidRequest_ShouldReturnCreated()
    {
        // Given
        var request  = CreateCartRequestFaker.GenerateValidRequest();

        // When
        var response = await _client.PostAsJsonAsync("api/carts", request);

        // Then
        response.EnsureSuccessStatusCode();
        var createCartResponse = await response.Content.ReadFromJsonAsync<CreateCartResponse>();
        createCartResponse.Should().NotBeNull();
        createCartResponse.Id.Should().NotBeEmpty();
        createCartResponse.UserId.Should().Be(request.UserId);
        createCartResponse.Date.Should().Be(request.Date);
    }
}