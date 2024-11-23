using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Carts.CreateCart;
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

        var command = _mapper.Map<CreateCartCommand>(request);
        var result = await _mediator.Send(command);

        //TODO: Add location header properly pointing to the get resource endpoint
        return CreatedAtAction(nameof(CreateCart),
            new { id = result.Id }, _mapper.Map<CreateCartResponse>(result));
    }
}