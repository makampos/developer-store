using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Integration.Configurations;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
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
    public async Task CreateCart_WithValidRequest_ShouldReturnCreated()
    {
        // Given
        var request  = CreateCartRequestFaker.GenerateValidRequest();

        // When
        var response = await _client.PostAsJsonAsync("api/carts", request);

        // Then
        response.EnsureSuccessStatusCode();


        var createCartResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>();
        var id = createCartResponse!.Data!.Id;
        response.Headers.Location!.LocalPath.Should().ContainAny($"/api/Carts/{id}");
        createCartResponse.Should().NotBeNull();
        createCartResponse!.Data.Should().BeEquivalentTo(request);
        createCartResponse!.Success.Should().BeTrue();
        createCartResponse.Errors.Should().BeNullOrEmpty();
    }

    [Fact(DisplayName = "Given a valid request, When getting a cart, Then it should return Ok StatusCode " +
                        "and a cart response")]
    public async Task GetCart_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createCartRequest = CreateCartRequestFaker.GenerateValidRequest();
        var createCartResponse = await _client.PostAsJsonAsync("api/carts", createCartRequest);
        createCartResponse.EnsureSuccessStatusCode();

        var cartId = await createCartResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>();

        // When
        var id = cartId!.Data!.Id;
        var response = await _client.GetAsync($"api/carts/{id}");

        // Then
        response.EnsureSuccessStatusCode();
        var getCartResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<GetCartResponse>>();

        getCartResponse.Should().NotBeNull();
        getCartResponse!.Data.Should().BeEquivalentTo(createCartRequest);
        getCartResponse!.Success.Should().BeTrue();
        getCartResponse!.Message.Should().Be("Cart retrieved successfully");
        getCartResponse.Errors.Should().BeNullOrEmpty();
    }

    [Fact(DisplayName = "Given an invalid request, When creating a cart, Then it should return BadRequest StatusCode " +
                        "and a error response")]
    public async Task CreateCart_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Given
        var request = DeleteCartRequestFaker.GenerateInvalidRequest();

        // When
        var response = await _client.DeleteAsync($"api/carts/{request.Id}");
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorResponse.Should().NotBeNull();
        errorResponse!.Success.Should().BeFalse();
        errorResponse!.Message.Should().Be("Id is required");
    }

    [Fact(DisplayName = "Given a valid request, When deleting a cart, Then it should return StatusCode Ok")]
    public async Task DeleteCart_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createCartRequest = CreateCartRequestFaker.GenerateValidRequest();
        var createCartResponse = await _client.PostAsJsonAsync("api/carts", createCartRequest);
        createCartResponse.EnsureSuccessStatusCode();

        var createCarResponse = await createCartResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>();

        // When
        var id = createCarResponse!.Data!.Id;
        var response = await _client.DeleteAsync($"api/carts/{id}");
        response.EnsureSuccessStatusCode();
        var deleteResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Then
        deleteResponse.Should().NotBeNull();
        deleteResponse!.Success.Should().BeTrue();
        deleteResponse!.Message.Should().Be("Cart deleted successfully");
    }
}