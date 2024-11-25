using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Integration.Configurations;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Controllers;

[Collection("Integration Tests")]
public class SalesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public SalesControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.InitializeAsync().GetAwaiter().GetResult();
        _client = _factory.CreateClient();
    }

    [Fact(DisplayName = "Given a valid request, When creating a sale, Should return Created StatusCode" +
                        "and a sale response")]
    public async Task CreateSale_WithValidRequest_ShouldReturnCreated()
    {
        // Given
        var createProductRequests = CreateProductRequestFaker.GenerateValidRequests(4);

        var productIds = new List<Guid>();

        foreach (var request in createProductRequests)
        {
            var createdProductResponse = await _client.PostAsJsonAsync("api/Product", request);
            createdProductResponse.EnsureSuccessStatusCode();
            var id = await createdProductResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>()
                .ContinueWith(x => x.Result!.Data!.Id);
            productIds.Add(id);
        }

        var saleRequest = CreateSaleRequestFaker.GenerateValidRequest(productIds);

        // When
        var createdSalesResponse = await _client.PostAsJsonAsync("api/sale", saleRequest);
        createdSalesResponse.EnsureSuccessStatusCode();
        var createdSale = await createdSalesResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();

        // Then
        createdSale.Should().NotBeNull();
        // TODO: Assert result properties

    }
}