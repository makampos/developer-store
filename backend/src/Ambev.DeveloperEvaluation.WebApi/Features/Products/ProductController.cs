using Ambev.DeveloperEvaluation.Application.Products.Categories;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProductsByCategory;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Categories.GetAllCategories;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProductsByCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IMediator mediator, IMapper mapper, ILogger<ProductController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProductsAsync([FromQuery] GetAllProductsRequest request, CancellationToken
        cancellationToken)
    {
        _logger.LogInformation("Controller {ProductController} triggered to handle {GetProductsRequest}",
            nameof(ProductController), nameof(GetAllProductsRequest));

        var validator = new GetAllProductsRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {GetProductsRequest}", nameof(GetAllProductsRequest));
            return base.BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<GetAllProductsCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return OkPaginated(new PaginatedList<GetProductResponse>(
            _mapper.Map<List<GetProductResponse>>(result.Products.Items),
            result.Products.TotalCount,
            result.Products.CurrentPage,
            result.Products.PageSize));
    }

    [HttpGet("{id:guid}", Name = nameof(GetProductAsync))]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProductAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Controller {ProductController} triggered to handle {GetProductRequest}",
            nameof(ProductController), nameof(GetProductRequest));

        var request = GetProductRequest.Create(id);
        var validator = new GetProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {GetProductRequest}", nameof(GetProductRequest));
            return base.BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<GetProductCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<GetProductResponse>()
        {
            Data = _mapper.Map<GetProductResponse>(response),
            Success = true,
            Message = "Product retrieved successfully"
        });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductRequest request, CancellationToken
        cancellationToken)
    {
        _logger.LogInformation("Controller {ProductController} triggered to handle {CreateProductRequest}",
            nameof(ProductController), nameof(CreateProductRequest));

        var validator = new CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            //TODO: Implement BadRequest correctly
            _logger.LogWarning($"Validation failed for {nameof(CreateProductRequest)}");
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateProductCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(nameof(GetProductAsync),
            new { id = response.Id }, _mapper.Map<CreateProductResponse>(response));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid id, [FromBody] UpdateProductRequest request,
    CancellationToken cancellationToken)
    {
        _logger.LogInformation("Controller {ProductController} triggered to handle {UpdateProductRequest}",
            nameof(ProductController), nameof(UpdateProductRequest));

        request = request.IncludeId(id);

        var validator = new UpdateProductRequestValidator(id);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            //TODO: Implement BadRequest correctly
            _logger.LogWarning("Validation failed for {UpdateProductRequest}", nameof(UpdateProductRequest));
            return base.BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<UpdateProductCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        //TODO: Implement ApiResponseWithData
        return base.Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Controller {ProductController} triggered to handle {DeleteProductRequest}",
            nameof(ProductController), nameof(DeleteProductRequest));

        var request = DeleteProductRequest.Create(id);
        var validator = new DeleteProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            //TODO: Implement BadRequest correctly
            _logger.LogWarning("Validation failed for {DeleteProductRequest}", nameof(DeleteProductRequest));
            return base.BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<DeleteProductCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return base.Ok(new ApiResponse
        {
            Success = response.Success,
            Message = "Product deleted successfully"
        });
    }

    [HttpGet("categories")]
    [ProducesResponseType(typeof(ApiResponseWithData<IReadOnlyList<string>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Controller {ProductController} triggered to handle {GetAllCategoriesRequest}",
            nameof(ProductController),nameof(GetAllCategoriesRequest));

        var command = new GetAllProductCategoriesCommand();
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<IReadOnlyList<string>>()
        {
            Data = result.ProductCategories,
            Success = true,
            Message = "Categories retrieved successfully"
        });
    }

    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(PaginatedResponse<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllProductsByCategoryAsync([FromRoute] string category, [FromQuery]
        GetAllProductsByCategoryRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Controller {ProductController} triggered to handle {GetProductsByCategoryRequest}",
            nameof(ProductController), nameof(GetAllProductsByCategoryRequest));

        request = request.IncludeCategory(category);

        var validator = new GetAllProductsByCategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {GetProductsByCategoryRequest}", nameof(GetAllProductsByCategoryRequest));
            return base.BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<GetAllProductsByCategoryCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return OkPaginated(new PaginatedList<GetProductResponse>(
            _mapper.Map<List<GetProductResponse>>(result.Products.Items),
            result.Products.TotalCount,
            result.Products.CurrentPage,
            result.Products.PageSize));
    }
}