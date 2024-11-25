using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSale;

public class GetAllSaleHandler : IRequestHandler<GetAllSaleCommand, GetAllSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllSaleHandler> _logger;

    public GetAllSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<GetAllSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetAllSaleResult> Handle(GetAllSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {GetAllSaleHandler} triggered to handle {GetAllSaleCommand} " +
            "with PageNumber: {PageNumber} and PageSize: {PageSize}", nameof(GetAllSaleHandler),
            nameof(GetAllSaleCommand), request.PageNumber, request.PageSize);

        var pagedResultOfSales = await _saleRepository.GetAllAsync(request.PageNumber, request.PageSize,
            request.Order, cancellationToken);

        var map = _mapper.Map<GetAllSaleResult>(pagedResultOfSales);
        return map;
    }
}