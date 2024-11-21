using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(product => product.Title).NotEmpty().Length(3, 50);
        RuleFor(product => product.Price).GreaterThan(0);
        RuleFor(product => product.Description).NotEmpty().Length(3, 500);
        RuleFor(product => product.Category).NotEmpty();
        RuleFor(product => product.Image).NotEmpty();
        RuleFor(product => product.Rating).NotNull(); // TODO: Add Rating validation
    }
}