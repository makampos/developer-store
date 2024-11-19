using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
         RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
         // TODO: Add validation rules for Price, Description, Category, Image, and Rating etc.
    }
}