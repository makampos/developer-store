using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;

public class GetAllProductsRequestValidator : AbstractValidator<GetAllProductsRequest>
{
    public GetAllProductsRequestValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0).When(x => x.PageNumber.HasValue);
        RuleFor(x => x.PageSize).GreaterThan(0).When(x => x.PageSize.HasValue);
    }
}