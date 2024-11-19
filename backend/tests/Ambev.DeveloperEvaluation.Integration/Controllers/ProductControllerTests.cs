using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Integration.Configurations;
using Ambev.DeveloperEvaluation.Integration.TestData;
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
        // Arrange
        var request = CreateProductRequestFaker.GenerateValidRequest();

        // Act
        var response = await _client.PostAsJsonAsync("/api/Product", request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "Given an invalid request, When creating a product, Should return BadRequest StatusCode")]
    public async Task CreateProduct_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Arrange
        var request = CreateProductRequestFaker.GenerateInvalidRequest();

        // Act
        var response = await _client.PostAsJsonAsync("/api/Product", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


}