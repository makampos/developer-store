using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProductsByCategory;

public class GetAllProductsByCategoryHandler : IRequestHandler<GetAllProductsByCategoryCommand, GetAllProductsByCategoryResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductsByCategoryHandler> _logger;

    public GetAllProductsByCategoryHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetAllProductsByCategoryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetAllProductsByCategoryResult> Handle(GetAllProductsByCategoryCommand command, CancellationToken
            cancellationToken)
    {
        _logger.LogInformation("Handler {GetAllProductsByCategoryHandler} triggered to handle {GetAllProductsByCategoryCommand} " +
            "with Category: {Category} and PageNumber: {PageNumber} and PageSize: {PageSize}", nameof(GetAllProductsByCategoryHandler),
            nameof(GetAllProductsByCategoryCommand), command.Category, command.PageNumber, command.PageSize);

        //TODO: Implement Validator

        var pagedResult = await _productRepository.GetAllAsync(command.PageNumber, command.PageSize,
            command.Order, command.Category, cancellationToken);

        _logger.LogInformation("Products retrieved successfully");

        return _mapper.Map<GetAllProductsByCategoryResult>(pagedResult);
    }
}