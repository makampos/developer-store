using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Integration.Configurations;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Controllers;

[Collection("Integration Tests")]
public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ProductControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.InitializeAsync().GetAwaiter().GetResult();
        _client = _factory.CreateClient();
    }

    [Fact(DisplayName = "Given a valid request, When creating a product, Should return Created StatusCode")]
    public async Task CreateProduct_WithValidRequest_ShouldReturnCreated()
    {
        // Given
        var request = CreateProductRequestFaker.GenerateValidRequest();

        // When
        var response = await _client.PostAsJsonAsync("/api/Product", request);

        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "Given an invalid request, When creating a product, Should return BadRequest StatusCode")]
    public async Task CreateProduct_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Given
        var request = CreateProductRequestFaker.GenerateInvalidRequest();

        // When
        var response = await _client.PostAsJsonAsync("/api/Product", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Given a valid request, When updating a product, Should return Ok StatusCode")]
    public async Task UpdateProduct_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createRequest = CreateProductRequestFaker.GenerateValidRequest();
        var createResponse = await _client.PostAsJsonAsync("/api/Product", createRequest);
        createResponse.EnsureSuccessStatusCode();

        var id = await createResponse.Content
            .ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>()
            .ContinueWith(x => x.Result!.Data!.Id);

        var request = UpdateProductRequestFaker.GenerateValidRequest();

        // When
        var response = await _client.PutAsJsonAsync($"/api/Product/{id}", request);

        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // TODO: GetById and compare the updated product with the request
    }

    [Fact(DisplayName = "Given an invalid request, When updating a product, Should return BadRequest StatusCode")]
    public async Task UpdateProduct_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Given
        var request = UpdateProductRequestFaker.GenerateInvalidRequest();
        var id = Guid.NewGuid();

        // When
        var response = await _client.PutAsJsonAsync($"/api/Product/{id}", request);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Given an invalid request, When deleting a product, Should return BadRequest StatusCode")]
    public async Task DeleteProduct_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Given
        var id = Guid.Empty;

        // When
        var response = await _client.DeleteAsync($"/api/Product/{id}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Given a valid request, When deleting a product, Should return Ok StatusCode")]
    public async Task DeleteProduct_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createRequest = CreateProductRequestFaker.GenerateValidRequest();
        var createResponse = await _client.PostAsJsonAsync("/api/Product", createRequest);
        createResponse.EnsureSuccessStatusCode();

        var id = await createResponse.Content
            .ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>()
            .ContinueWith(x => x.Result!.Data!.Id);

        // When
        var response = await _client.DeleteAsync($"/api/Product/{id}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var deleteResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
        deleteResponse!.Message.Should().Be("Product deleted successfully");
        deleteResponse.Success.Should().BeTrue();
        deleteResponse.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given a valid request, When getting a product, Should return Ok StatusCode")]
    public async Task GetProduct_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createRequest = CreateProductRequestFaker.GenerateValidRequest();
        var createResponse = await _client.PostAsJsonAsync("/api/Product", createRequest);
        createResponse.EnsureSuccessStatusCode();

        var createdProductResponse = await createResponse.Content
            .ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>()
            .ContinueWith(x => x.Result!.Data!);

        var id = createdProductResponse.Id;

        // When
        var response = await _client.GetAsync($"/api/Product/{id}");

        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsStringAsync();
        //TODO: Improve ApiResponseWithData because currently returns two instances of 'data' property
    }

    [Fact(DisplayName = "Given an invalid request, When getting a product, Should return BadRequest StatusCode")]
    public async Task GetProduct_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Given
        var id = Guid.Empty;

        // When
        var response = await _client.GetAsync($"/api/Product/{id}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}