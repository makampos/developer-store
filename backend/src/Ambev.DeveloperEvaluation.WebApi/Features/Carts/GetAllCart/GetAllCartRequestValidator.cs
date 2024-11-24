using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetAllCart;

public class GetAllCartRequestValidator : AbstractValidator<GetAllCartRequest>
{
    public GetAllCartRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.PageNumber).GreaterThan(0).When(x => x.PageNumber.HasValue)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize).GreaterThan(0).When(x => x.PageSize.HasValue)
            .WithMessage("Page size must be greater than 0");

        // RuleFor(x => x.Order)
        //     .Matches("^(asc|desc)$")
        //     .When(x => x.Order != null)
        //     .WithMessage("Order must be 'asc' or 'desc'");
    }
}