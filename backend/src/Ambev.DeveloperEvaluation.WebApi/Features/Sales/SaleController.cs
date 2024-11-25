using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
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

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllSalesAsync([FromQuery] GetAllSaleRequest request)
    {
        _logger.LogInformation("Controller {SaleController} triggered to handle {GetAllSaleRequest}",
            nameof(SaleController), nameof(GetAllSaleRequest));

        var command = _mapper.Map<GetAllSaleCommand>(request);
        var result = await _mediator.Send(command);

        return OkPaginated(new PaginatedList<GetSaleResponse>(
            _mapper.Map<List<GetSaleResponse>>(result.Sales.Items),
            result.Sales.TotalCount,
            result.Sales.CurrentPage,
            result.Sales.PageSize));
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

        return Created(nameof(GetSaleAsync),
            new { id = result.Id }, map);
    }

    [HttpGet("{id:int}", Name = nameof(GetSaleAsync))]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaleAsync([FromRoute] int id)
    {
        _logger.LogInformation("Controller {SaleController} triggered to handle {GetSaleRequest}",
            nameof(SaleController), nameof(GetSaleRequest));

        var getSaleRequest = GetSaleRequest.Create(id);
        var getSaleCommand = _mapper.Map<GetSaleCommand>(getSaleRequest);
        var result = await _mediator.Send(getSaleCommand);
        var response = _mapper.Map<GetSaleResponse>(result);

        return Ok(new ApiResponseWithData<GetSaleResponse>()
        {
            Data = response,
            Success = true,
            Message = "Sale retrieved successfully"
        });
    }

    [HttpDelete("{id:int}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSaleAsync([FromRoute] int id)
    {
        _logger.LogInformation("Controller {SaleController} triggered to handle {CancelSaleRequest}",
            nameof(SaleController), nameof(CancelSaleRequest));

        var cancelSaleRequest = CancelSaleRequest.Create(id);
        var cancelSaleCommand = _mapper.Map<CancelSaleCommand>(cancelSaleRequest);
        var result = await _mediator.Send(cancelSaleCommand);
        var response = _mapper.Map<CancelSaleResponse>(result);

        _logger.LogInformation("Sale canceled with id {SaleId}", id);

        return Ok(new ApiResponseWithData<CancelSaleResponse>()
        {
            Data = response,
            Success = true,
            Message = response.Message
        });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleAsync([FromRoute] int id, [FromBody] UpdateSaleRequest request)
    {
        _logger.LogInformation("Controller {SaleController} triggered to handle {UpdateSaleRequest}",
            nameof(SaleController), nameof(UpdateSaleRequest));

        var updateSaleRequest = request.IncludeId(id);
        var updateSaleCommand = _mapper.Map<UpdateSaleCommand>(updateSaleRequest);
        var result = await _mediator.Send(updateSaleCommand);

        _logger.LogInformation("Sale updated with id {SaleId}", id);

        return Ok(new ApiResponseWithData<UpdateSaleResponse>()
        {
            Data = _mapper.Map<UpdateSaleResponse>(result),
            Success = true,
        });
    }


}