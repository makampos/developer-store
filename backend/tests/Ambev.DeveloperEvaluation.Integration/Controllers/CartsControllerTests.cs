using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Integration.Configurations;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetAllCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Controllers;


[CollectionDefinition(nameof(CartsControllerTestsCollection))]
public class CartsControllerTestsCollection : ICollectionFixture<IntegrationTestFactory>;

[Collection(nameof(CartsControllerTestsCollection))]
public class CartsControllerTests : DatabaseTest
{
    public CartsControllerTests(IntegrationTestFactory factory) : base(factory){ }

    [Fact(DisplayName = "Given a valid request, When creating a cart, Then it should return Created StatusCode " +
                        "and a cart response")]
    public async Task CreateCart_WithValidRequest_ShouldReturnCreated()
    {
        // Given
        var request  = CreateCartRequestFaker.GenerateValidRequest();

        // When
        var response = await Client.PostAsJsonAsync("api/carts", request);

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
        var createCartResponse = await Client.PostAsJsonAsync("api/carts", createCartRequest);
        createCartResponse.EnsureSuccessStatusCode();

        var cartId = await createCartResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>();

        // When
        var id = cartId!.Data!.Id;
        var response = await Client.GetAsync($"api/carts/{id}");

        // Then
        response.EnsureSuccessStatusCode();
        var getCartResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<GetCartResponse>>();

        getCartResponse.Should().NotBeNull();
        getCartResponse!.Data.Should().NotBeNull();
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
        var response = await Client.DeleteAsync($"api/carts/{request.Id}");
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
        var createCartResponse = await Client.PostAsJsonAsync("api/carts", createCartRequest);
        createCartResponse.EnsureSuccessStatusCode();

        var createCarResponse = await createCartResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>();

        // When
        var id = createCarResponse!.Data!.Id;
        var response = await Client.DeleteAsync($"api/carts/{id}");
        response.EnsureSuccessStatusCode();
        var deleteResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        // Then
        deleteResponse.Should().NotBeNull();
        deleteResponse!.Success.Should().BeTrue();
        deleteResponse!.Message.Should().Be("Cart deleted successfully");
    }

    [Fact(DisplayName = "Given a valid request, When getting all carts, Then it should return Ok StatusCode " +
                        "and a paginated list of carts")]
    public async Task GetAllCart_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createCartRequest = CreateCartRequestFaker.GenerateValidRequest();
        var createCartResponse = await Client.PostAsJsonAsync("api/carts", createCartRequest);
        createCartResponse.EnsureSuccessStatusCode();

        // When
        var getAllCartsRequest = GetAllCartRequest.Create(1, 10);
        var response = await Client.GetAsync($"api/carts?pageNumber={getAllCartsRequest.PageNumber}&pageSize={getAllCartsRequest.PageSize}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<GetCartResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNullOrEmpty();
        result!.Data.Should().HaveCount(1);
        result!.Success.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(1);
        result.TotalPages.Should().Be(1);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
        result.CurrentPage.Should().Be(1);
    }

    [Fact(DisplayName =
        "Given an invalid request, When getting all carts, Then it should return BadRequest StatusCode " +
        "and a error response")]
    public async Task GetAllCart_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Given
        var getAllCartsRequest = GetAllCartRequest.Create(-1, -10);

        // When
        var response = await Client.GetAsync($"api/carts?pageNumber={getAllCartsRequest.PageNumber}&pageSize={getAllCartsRequest.PageSize}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
        errorResponse.Should().NotBeNull();
        errorResponse!.Success.Should().BeFalse();
        errorResponse!.Message.Should().Be("Page number must be greater than 0");
    }

    [Fact(DisplayName =
        "Given a valid request with ordering, When getting all carts, Then it should return Ok StatusCode " +
        "and a paginated list of carts ordered by the specified field")]
    public async Task GetAllCart_WithOrdering_ShouldReturnOk()
    {
        // Given
        var createCartRequests = CreateCartRequestFaker.GenerateValidRequests(3);

        foreach (var createCartRequest in createCartRequests)
        {
            var createCartResponse = await Client.PostAsJsonAsync("api/carts", createCartRequest);
            createCartResponse.EnsureSuccessStatusCode();
        }

        var request = GetAllCartRequest.Create(1, 10, "date asc");

        // When
        var response = await Client.GetAsync($"api/carts?pageNumber={request.PageNumber}&pageSize={request.PageSize}&order={request.Order}");
        response.EnsureSuccessStatusCode();

        // Then
        var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<GetCartResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeNullOrEmpty();
        result!.Data.Should().HaveCount(3);
        result!.Data.Should().BeInAscendingOrder(x => x.Date);
        result!.Success.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(3);
        result.TotalPages.Should().Be(1);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
        result.CurrentPage.Should().Be(1);
    }

    [Fact(DisplayName = "Given a valid request, When updating a cart, Then it should return Ok StatusCode " +
                        "and a cart response")]
    public async Task UpdateCart_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createCartRequest = CreateCartRequestFaker.GenerateValidRequest();
        var createCartResponse = await Client.PostAsJsonAsync("api/carts", createCartRequest);
        createCartResponse.EnsureSuccessStatusCode();

        var cartId = await createCartResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>();

        var updateCartRequest = UpdateCartRequestFaker.GenerateValidRequest();

        // When
        var id = cartId!.Data!.Id;
        var response = await Client.PutAsJsonAsync($"api/carts/{id}", updateCartRequest);

        // Then
        response.EnsureSuccessStatusCode();
        var updateCartResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<UpdateCartResponse>>();

        updateCartResponse.Should().NotBeNull();
        updateCartResponse!.Data.Should().NotBeNull();
        updateCartResponse!.Success.Should().BeTrue();
        updateCartResponse!.Message.Should().Be("Cart updated successfully");
        updateCartResponse.Errors.Should().BeNullOrEmpty();
    }

    [Fact(DisplayName = "Given an invalid request, When updating a cart, Then it should return BadRequest StatusCode " +
                        "and a error response")]
    public async Task UpdateCart_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Given
        var createCartRequest = CreateCartRequestFaker.GenerateValidRequest();
        var createCartResponse = await Client.PostAsJsonAsync("api/carts", createCartRequest);
        createCartResponse.EnsureSuccessStatusCode();

        var cartId = await createCartResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateCartResponse>>();

        var updateCartRequest = UpdateCartRequestFaker.GenerateInvalidRequest();

        // When

        var id = cartId!.Data!.Id;
        var response = await Client.PutAsJsonAsync($"api/carts/{id}", updateCartRequest);

        // Then
        var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
        errorResponse.Should().NotBeNull();
        errorResponse!.Success.Should().BeFalse();
        errorResponse!.Message.Should().Be("CartItems is required");
    }
}