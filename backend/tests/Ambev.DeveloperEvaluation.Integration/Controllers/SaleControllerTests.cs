using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Integration.Configurations;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Controllers;

[CollectionDefinition(nameof(SaleControllerTestsCollection))]
public class SaleControllerTestsCollection : ICollectionFixture<IntegrationTestFactory>;

[Collection(nameof(SaleControllerTestsCollection))]
public class SaleControllerTests : DatabaseTest
{
    public SaleControllerTests(IntegrationTestFactory factory) : base(factory){ }

    [Fact(DisplayName = "Given a valid request, When creating a sale, Should return Created StatusCode" +
                        "and a sale response")]
    public async Task CreateSale_WithValidRequest_ShouldReturnCreated()
    {
        // Given
        var createProductRequests = CreateProductRequestFaker.GenerateValidRequests(4);

        var productIds = new List<Guid>();

        foreach (var request in createProductRequests)
        {
            var createdProductResponse = await Client.PostAsJsonAsync("api/Product", request);
            createdProductResponse.EnsureSuccessStatusCode();
            var id = await createdProductResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>()
                .ContinueWith(x => x.Result!.Data!.Id);
            productIds.Add(id);
        }

        var saleRequest = CreateSaleRequestFaker.GenerateValidRequest(productIds);

        // When
        var createdSalesResponse = await Client.PostAsJsonAsync("api/Sale", saleRequest);
        createdSalesResponse.EnsureSuccessStatusCode();
        var createdSale = await createdSalesResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        var saleId = createdSale!.Data!.Id;
        // Then
        createdSale.Should().NotBeNull();
        createdSalesResponse.Headers.Location!.LocalPath.Should().ContainAny($"/api/Sale/{saleId}");

        createdSale.Success.Should().BeTrue();
        createdSale.Errors.Should().BeNullOrEmpty();
        createdSale.Data.Should().NotBeNull();
    }

    [Fact(DisplayName = "Given a valid request, When getting a sale, Should return Ok StatusCode" +
                        "and a sale response")]
    public async Task GetSale_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createProductRequests = CreateProductRequestFaker.GenerateValidRequests(1);
        var productId = Guid.Empty;
        foreach(var request in createProductRequests)
        {
            var createdProductResponse = await Client.PostAsJsonAsync("api/Product", request);
            createdProductResponse.EnsureSuccessStatusCode();
            productId = await createdProductResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>()
                .ContinueWith(x => x.Result!.Data!.Id);
        }

        var createSaleRequest = CreateSaleRequestFaker.GenerateValidRequest([productId]);
        var createdSaleResponse = await Client.PostAsJsonAsync("api/Sale", createSaleRequest);
        createdSaleResponse.EnsureSuccessStatusCode();
        var createdSale = await createdSaleResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();

        // When
        var saleId = createdSale!.Data!.Id;
        var getSaleResponse = await Client.GetAsync($"api/Sale/{saleId}");

        // Then
        getSaleResponse.EnsureSuccessStatusCode();
        var getSale = await getSaleResponse.Content.ReadFromJsonAsync<ApiResponseWithData<GetSaleResponse>>();
        getSale.Success.Should().BeTrue();
        getSale.Errors.Should().BeNullOrEmpty();
        getSale.Message.Should().Be("Sale retrieved successfully");
        getSale!.Data.Should().NotBeNull();
    }

    [Fact(DisplayName = "Given a valid request, When canceling a sale, Should return Ok StatusCode" +
                        "and a sale response message")]
    public async Task CancelSale_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createProductRequests = CreateProductRequestFaker.GenerateValidRequests(1);
        var productId = Guid.Empty;
        foreach(var request in createProductRequests)
        {
            var createdProductResponse = await Client.PostAsJsonAsync("api/Product", request);
            createdProductResponse.EnsureSuccessStatusCode();
            productId = await createdProductResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>()
                .ContinueWith(x => x.Result!.Data!.Id);
        }

        var createSaleRequest = CreateSaleRequestFaker.GenerateValidRequest([productId]);
        var createdSaleResponse = await Client.PostAsJsonAsync("api/Sale", createSaleRequest);
        createdSaleResponse.EnsureSuccessStatusCode();
        var createdSale = await createdSaleResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();

        // When
        var saleId = createdSale!.Data!.Id;
        var cancelSaleResponse = await Client.DeleteAsync($"api/Sale/{saleId}/cancel");

        // Then
        cancelSaleResponse.EnsureSuccessStatusCode();
        var cancelSale = await cancelSaleResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CancelSaleResponse>>();
        cancelSale!.Success.Should().BeTrue();
        cancelSale.Errors.Should().BeNullOrEmpty();
        cancelSale.Data.Should().NotBeNull();
        cancelSale.Data!.Message.Should().Be("Sale canceled successfully");
    }

    [Fact(DisplayName = "Given a valid request, When updating a sale, Should return Ok StatusCode" +
                        "and a sale response")]
    public async Task UpdateSale_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createProductRequests = CreateProductRequestFaker.GenerateValidRequests(1);
        var productId = Guid.Empty;
        foreach(var request in createProductRequests)
        {
            var createdProductResponse = await Client.PostAsJsonAsync("api/Product", request);
            createdProductResponse.EnsureSuccessStatusCode();
            productId = await createdProductResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>()
                .ContinueWith(x => x.Result!.Data!.Id);
        }

        var createSaleRequest = CreateSaleRequestFaker.GenerateValidRequest([productId]);

        var createdSaleResponse = await Client.PostAsJsonAsync("api/Sale", createSaleRequest);
        createdSaleResponse.EnsureSuccessStatusCode();
        var createdSale = await createdSaleResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();

        // When
        var saleId = createdSale!.Data!.Id;
        var updateSaleRequest = UpdateSaleRequestFaker.GenerateValidRequest(saleId, productId);
        var updateSaleResponse = await Client.PutAsJsonAsync($"api/Sale/{saleId}", updateSaleRequest);

        // Then
        updateSaleResponse.EnsureSuccessStatusCode();
        var updatedSale = await updateSaleResponse.Content.ReadFromJsonAsync<ApiResponseWithData<UpdateSaleResponse>>();
        updatedSale!.Success.Should().BeTrue();
        updatedSale.Errors.Should().BeNullOrEmpty();
        updatedSale.Data.Should().NotBeNull();
    }

    [Fact(DisplayName = "Given a valid request, When getting all sales, Should return Ok StatusCode" +
                        "and a paginated response")]
    public async Task GetAllSales_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createProductRequests = CreateProductRequestFaker.GenerateValidRequests(1);
        var productId = Guid.Empty;
        foreach(var createProductRequest in createProductRequests)
        {
            var createdProductResponse = await Client.PostAsJsonAsync("api/Product", createProductRequest);
            createdProductResponse.EnsureSuccessStatusCode();
            productId = await createdProductResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>()
                .ContinueWith(x => x.Result!.Data!.Id);
        }

        var createSaleRequest = CreateSaleRequestFaker.GenerateValidRequest([productId]);
        var createdSaleResponse = await Client.PostAsJsonAsync("api/Sale", createSaleRequest);
        createdSaleResponse.EnsureSuccessStatusCode();
        var request = GetAllSaleRequest.Create(1, 10, "saleDate");

        // When
        var getAllSalesResponse = await Client.GetAsync(
            $"api/Sale?pageNumber={request.PageNumber}&pageSize={request.PageSize}&order={request.Order}");
        getAllSalesResponse.EnsureSuccessStatusCode();
        var getAllSales = await getAllSalesResponse.Content.ReadFromJsonAsync<PaginatedResponse<GetSaleResponse>>();

        // Then
        getAllSales!.Success.Should().BeTrue();
        getAllSales.Errors.Should().BeNullOrEmpty();
        getAllSales.Data.Should().NotBeNull();
        getAllSales.Data!.Should().NotBeEmpty();
        getAllSales.Data!.Should().HaveCount(1);
    }
}