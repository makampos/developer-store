using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Categories;

public record GetAllProductCategoriesCommand() : IRequest<GetAllProductCategoriesResult>;