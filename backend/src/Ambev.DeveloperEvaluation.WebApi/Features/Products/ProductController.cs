using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;
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
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetAllProductsResponse>>), StatusCodes.Status200OK)]
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
        var response = await _mediator.Send(command, cancellationToken);

        //TODO: Add mapper

        var paginatedList = new PaginatedList<Product>(
            (List<Product>)response.Products.Items,
            response.Products.TotalCount,
            response.Products.CurrentPage,
            response.Products.PageSize);

        return OkPaginated(paginatedList);
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
}