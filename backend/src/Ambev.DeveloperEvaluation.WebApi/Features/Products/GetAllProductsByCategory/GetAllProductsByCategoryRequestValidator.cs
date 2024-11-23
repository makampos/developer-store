using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProductsByCategory;

public class GetAllProductsByCategoryRequestValidator : AbstractValidator<GetAllProductsByCategoryRequest>
{
    public GetAllProductsByCategoryRequestValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required");

        RuleFor(x => x.PageNumber).GreaterThan(0).When(x => x.PageNumber.HasValue);
        RuleFor(x => x.PageSize).GreaterThan(0).When(x => x.PageSize.HasValue);

        // RuleFor(x => x.Order)
        //     .NotEmpty()
        //     .When(x => x.Order != null)
        //     .WithMessage("Order is required")
        //     .Matches(@"^(\w+|asc|desc)$")
        //     .WithMessage("Order must contain at least asc, desc or just the field name");
    }
}