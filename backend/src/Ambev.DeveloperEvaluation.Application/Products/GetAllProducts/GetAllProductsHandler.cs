using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsCommand, GetAllProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductsHandler> _logger;

    public GetAllProductsHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetAllProductsHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetAllProductsResult> Handle(GetAllProductsCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler {GetAllProductsHandler} triggered to handle {GetAllProductsCommand} " +
            "with PageNumber: {PageNumber} and PageSize: {PageSize}", nameof(GetAllProductsHandler),
            nameof(GetAllProductsCommand), command.PageNumber, command.PageSize);

        //TODO: Implement Validator

        var pagedResultOfProducts = await _productRepository.GetAllAsync(command.PageNumber, command.PageSize,
            command.Order, cancellationToken: cancellationToken);

        _logger.LogInformation("Products retrieved successfully");

        return _mapper.Map<GetAllProductsResult>(pagedResultOfProducts);
    }
}