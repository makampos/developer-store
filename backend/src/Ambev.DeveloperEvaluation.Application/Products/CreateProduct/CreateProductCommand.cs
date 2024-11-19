using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public record CreateProductCommand(
    string Title,
    decimal Price,
    string Description,
    string Category,
    string Image,
    Rating Rating) : IRequest<CreateProductResult>
{

    public CreateProductCommand() : this("", 0, "", "", "", Rating.Create(0, 0))
    { }

    public ValidationResultDetail Validate()
    {
        var validator = new CreateProductCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}