using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator(Guid id)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Id)
            .Equal(id)
            .WithMessage("Product ID must match route parameter");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Price is required");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required");

        RuleFor(x => x.Image)
            .NotEmpty()
            .WithMessage("Image is required");

        RuleFor(x => x.Rating.Rate).GreaterThanOrEqualTo(0)
            .WithMessage("Rating rate must be greater than or equal to 0");

        RuleFor(x => x.Rating.Count).GreaterThanOrEqualTo(0)
            .WithMessage("Rating count must be greater than or equal to 0");
    }
}