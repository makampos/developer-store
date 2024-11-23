using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Image).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Rating.Rate).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Rating.Count).GreaterThanOrEqualTo(0);
    }
}