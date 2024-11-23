using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Validation;

public class DeleteProductCommandValidatorTests
{
    private readonly DeleteProductCommandValidator _commandValidator = new();

    [Fact(DisplayName = "Given invalid id Then returns validation error")]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var id = Guid.Empty;
        var command = DeleteProductCommand.Create(id);

        var result = _commandValidator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}