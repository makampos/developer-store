using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Integration.Configurations;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
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
        var request = CreateProductRequestFaker.GenerateValidRequests(1)[0];

        // When
        var response = await _client.PostAsJsonAsync("/api/Product", request);

        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>();

        result!.Data!.Id.Should().NotBeEmpty();
        result.Success.Should().BeTrue();
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
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
    }

    [Fact(DisplayName = "Given a valid request, When updating a product, Should return Ok StatusCode")]
    public async Task UpdateProduct_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createRequest = CreateProductRequestFaker.GenerateValidRequests(1)[0];
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
        var createRequest = CreateProductRequestFaker.GenerateValidRequests(1)[0];
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
        var createRequest = CreateProductRequestFaker.GenerateValidRequests(1)[0];
        var createdResponse = await _client.PostAsJsonAsync("/api/Product", createRequest);
        createdResponse.EnsureSuccessStatusCode();

        var createdProductResponse = await createdResponse.Content
            .ReadFromJsonAsync<ApiResponseWithData<CreateProductResponse>>();

        var id = createdProductResponse!.Data!.Id;

        // When
        var getResponse = await _client.GetAsync($"/api/Product/{id}");

        // Then
        getResponse.EnsureSuccessStatusCode();
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getProductResponse = await getResponse.Content.ReadFromJsonAsync<ApiResponseWithData<GetProductResponse>>();

        getProductResponse.Should().NotBeNull();
        getProductResponse!.Success.Should().BeTrue();
        getProductResponse.Message.Should().Be("Product retrieved successfully");
        getProductResponse.Data.Should().BeEquivalentTo(createRequest);
        getProductResponse.Errors.Should().BeEmpty();
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

    [Fact(DisplayName = "Given a valid request, When getting all products, Should return Ok StatusCode")]
    public async Task GetAllProducts_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createdProductRequestList = CreateProductRequestFaker.GenerateValidRequests(6);

        foreach (var cpr in createdProductRequestList)
        {
            var createResponse = await _client.PostAsJsonAsync("/api/Product", cpr);
            createResponse.EnsureSuccessStatusCode();
        }

        var request = GetAllProductsRequest.Create(1,5);

        // When
        var response =
            await _client.GetAsync($"/api/Product?pageNumber={request.PageNumber}&pageSize={request.PageSize}");


        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<GetProductResponse>>();
        result.Should().NotBeNull();
        result!.PageSize.Should().Be(5);
        result.TotalCount.Should().Be(6);
        result.TotalPages.Should().Be(2);
        result.Data.Should().HaveCount(5);
        result.CurrentPage.Should().Be(1);
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact(DisplayName =
        "Given a valid request When getting all products with pageNumber and pageSize as null, Should return Ok StatusCode")]
    public async Task GetAllProducts_WithoutRequestObject_ShouldSetDefaultValues_And_ShouldReturnOk()
    {
        // Given
        var createdProductRequestList = CreateProductRequestFaker.GenerateValidRequests(2);

        foreach (var cpr in createdProductRequestList)
        {
            var createResponse = await _client.PostAsJsonAsync("/api/Product", cpr);
            createResponse.EnsureSuccessStatusCode();
        }

        // When
        var response =
            await _client.GetAsync($"/api/Product");

        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<GetProductResponse>>();
        result.Should().NotBeNull();
        result!.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
        result.TotalPages.Should().Be(1);
        result.Data.Should().HaveCount(2);
        result.CurrentPage.Should().Be(1);
        result.HasNextPage.Should().BeFalse();
        result.HasPreviousPage.Should().BeFalse();
    }


    [Fact(DisplayName = "Given a valid request, When getting all products ordered, Should return Ok StatusCode," +
                        " and ordered list by price descending and title ascending")]
    public async Task GetAllProducts_WithValidRequestAndOrder_ShouldReturnOk()
    {
        // Given
        var createdProductRequestList = CreateProductRequestFaker.GenerateValidRequests(3);

        foreach (var cpr in createdProductRequestList)
        {
            var createResponse = await _client.PostAsJsonAsync("/api/Product", cpr);
            createResponse.EnsureSuccessStatusCode();
        }

        var request = GetAllProductsRequest.Create(1, 5, "price desc, title asc");

        // When
        var response =
            await _client.GetAsync(
                $"/api/Product?pageNumber={request.PageNumber}&pageSize={request.PageSize}&order={request.Order}");

        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var orderedList = createdProductRequestList
            .OrderByDescending(x => x.Price)
            .ThenBy(x => x.Title)
            .ToList();

        var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<GetProductResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().BeEquivalentTo(orderedList, "because the list should be ordered by price " +
                                                          "descending and title ascending");
        result.PageSize.Should().Be(5);
        result.TotalCount.Should().Be(3);
        result.TotalPages.Should().Be(1);
        result.CurrentPage.Should().Be(1);
        result.HasNextPage.Should().BeFalse();
        result.HasPreviousPage.Should().BeFalse();
        result.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "Given a valid request, When getting all products ordered, and there are ties in price, " +
                        "should return Ok StatusCode, and ordered list applied primary order by price descending and " +
                        "secondary order by title ascending")]
    public async Task GetAllProducts_WithValidRequestAndOrderAndTieInPrice_ShouldReturnOk()
    {
        // Given
        var createdProductRequestList = CreateProductRequestFaker.GenerateValidRequestsWithTies(3);

        foreach (var cpr in createdProductRequestList)
        {
            var createResponse = await _client.PostAsJsonAsync("/api/Product", cpr);
            createResponse.EnsureSuccessStatusCode();
        }

        var request = GetAllProductsRequest.Create(1, 5, "price desc, title asc");

        // When
        var response =
            await _client.GetAsync(
                $"/api/Product?pageNumber={request.PageNumber}&pageSize={request.PageSize}&order={request.Order}");

        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var orderedList = createdProductRequestList
            .OrderByDescending(x => x.Price)
            .ThenBy(x => x.Title)
            .ToList();

        var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<GetProductResponse>>();
        result.Should().NotBeNull();
        result!.Data.Should().BeEquivalentTo(orderedList, "because the list should be ordered primarily by price and " +
                                                          "secondarily by title when there are ties in price");
        result.PageSize.Should().Be(5);
        result.TotalCount.Should().Be(3);
        result.TotalPages.Should().Be(1);
        result.CurrentPage.Should().Be(1);
        result.HasNextPage.Should().BeFalse();
        result.HasPreviousPage.Should().BeFalse();
        result.Success.Should().BeTrue();
        result.Message.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given a valid request, When getting all product categories, should return Ok StatusCode")]
    public async Task GetAllCategories_WithValidRequest_ShouldReturnOk()
    {
        // Given
        var createdProductRequestList = CreateProductRequestFaker.GenerateValidRequests(3);

        foreach (var cpr in createdProductRequestList)
        {
            var createResponse = await _client.PostAsJsonAsync("/api/Product", cpr);
            createResponse.EnsureSuccessStatusCode();
        }

        // When
        var response =
            await _client.GetAsync($"/api/Product/categories");

        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ApiResponseWithData<IReadOnlyList<string>>>();
        result.Should().NotBeNull();
        result!.Data.Should().NotBeEmpty();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Categories retrieved successfully");
        result.Errors.Should().BeEmpty();
    }
}

