using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetAllCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Carts.GetAllCart;
using Ambev.DeveloperEvaluation.WebApi.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Carts;

[ApiController]
[Route("api/[controller]")]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<CartsController> _logger;

    public CartsController(IMediator mediator, IMapper mapper, ILogger<CartsController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCartAsync([FromBody] CreateCartRequest request)
    {
        _logger.LogInformation("Controller {CartsController} triggered to handle {CreateCartRequest}",
            nameof(CartsController), nameof(CreateCartRequest));

        //TODO: Implement Validator for BadRequest response

        var command = _mapper.Map<CreateCartCommand>(request);
        var result = await _mediator.Send(command);

        return Created(nameof(GetCartAsync),
            new { id = result.Id }, _mapper.Map<CreateCartResponse>(result));
    }

    [HttpGet("{id:guid}", Name = nameof(GetCartAsync))]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCartAsync([FromRoute] Guid id)
    {
        _logger.LogInformation("Controller {CartsController} triggered to handle {GetCartRequest}",
            nameof(CartsController), nameof(GetCartRequest));

        var request = GetCartRequest.Create(id);

        var command = _mapper.Map<GetCartCommand>(request);
        var result = await _mediator.Send(command);

        return Ok(new ApiResponseWithData<GetCartResponse>()
        {
            Data = _mapper.Map<GetCartResponse>(result),
            Success = true,
            Message = "Cart retrieved successfully"
        });
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCartAsync([FromRoute] Guid id)
    {
        _logger.LogInformation("Controller {CartsController} triggered to handle {DeleteCartRequest}",
            nameof(CartsController), nameof(DeleteCartRequest));

        var request = DeleteCartRequest.Create(id);

        var validator = new DeleteCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {DeleteCartRequest}", nameof(DeleteCartRequest));
            return BadRequest(validationResult.Errors.FirstOrDefault()!.ErrorMessage);
        }

        var command = _mapper.Map<DeleteCartCommand>(request);
        var result = await _mediator.Send(command);

        return Ok(new ApiResponse()
        {
            Success = result.Success,
            Message = "Cart deleted successfully"
        });
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCartsAsync([FromQuery] GetAllCartRequest request)
    {
        _logger.LogInformation("Controller {CartsController} triggered to handle {GetCartRequest}",
            nameof(CartsController), nameof(GetCartRequest));

        var validator = new GetAllCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for {GetAllCartRequest}", nameof(GetAllCartRequest));
            return BadRequest(validationResult.Errors.FirstOrDefault()!.ErrorMessage);
        }

        var command = _mapper.Map<GetAllCardCommand>(request);
        var result = await _mediator.Send(command);

        return OkPaginated(new PaginatedList<GetCartResponse>(
            _mapper.Map<List<GetCartResponse>>(result.Carts.Items),
            result.Carts.TotalCount,
            result.Carts.CurrentPage,
            result.Carts.PageSize));
    }
}