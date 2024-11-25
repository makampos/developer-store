using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SaleController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<SaleController> _logger;

    public SaleController(IMediator mediator, IMapper mapper, ILogger<SaleController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSaleAsync([FromBody] CreateSaleRequest request)
    {
        _logger.LogInformation("Controller {SaleController} triggered to handle {CreateSaleRequest}",
            nameof(SaleController), nameof(CreateSaleRequest));

        //TODO: Implement validator

        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command);
        var map = _mapper.Map<CreateSaleResponse>(result);

        return Created(string.Empty,
            new { id = result.Id }, map);
    }
}