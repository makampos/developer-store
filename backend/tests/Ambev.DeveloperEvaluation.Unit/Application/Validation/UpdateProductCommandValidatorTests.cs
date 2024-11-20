namespace Ambev.DeveloperEvaluation.Unit.Application.Validation;

using Xunit;
using FluentValidation.TestHelper;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class UpdateProductCommandValidatorTests
{
    private readonly UpdateProductCommandValidator _validator = new();

    [Fact(DisplayName = "Given invalid id Then returns validation error")]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateProductCommand
        {
            Id = Guid.Empty
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact(DisplayName = "Given invalid title Then returns validation error")]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var command = new UpdateProductCommand { Title = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact(DisplayName = "Given invalid title Then returns validation error")]
    public void Should_Have_Error_When_Title_Is_Too_Long()
    {
        var command = new UpdateProductCommand { Title = new string('a', 101) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact(DisplayName = "Given invalid price Then returns validation error")]
    public void Should_Have_Error_When_Price_Is_Less_Than_Or_Equal_To_Zero()
    {
        var command = new UpdateProductCommand { Price = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact(DisplayName = "Given invalid description Then returns validation error")]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new UpdateProductCommand { Description = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact(DisplayName = "Given invalid description Then returns validation error")]
    public void Should_Have_Error_When_Description_Is_Too_Long()
    {
        var command = new UpdateProductCommand { Description = new string('a', 501) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact(DisplayName = "Given invalid category Then returns validation error")]
    public void Should_Have_Error_When_Category_Is_Empty()
    {
        var command = new UpdateProductCommand { Category = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Category);
    }

    [Fact(DisplayName = "Given invalid category Then returns validation error")]
    public void Should_Have_Error_When_Category_Is_Too_Long()
    {
        var command = new UpdateProductCommand { Category = new string('a', 51) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Category);
    }

    [Fact(DisplayName = "Given invalid image Then returns validation error")]
    public void Should_Have_Error_When_Image_Is_Empty()
    {
        var command = new UpdateProductCommand { Image = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Image);
    }

    [Fact(DisplayName = "Given invalid image Then returns validation error")]
    public void Should_Have_Error_When_Image_Is_Too_Long()
    {
        var command = new UpdateProductCommand { Image = new string('a', 301) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Image);
    }

    [Fact(DisplayName = "Given invalid rating rate Then returns validation error")]
    public void Should_Have_Error_When_Rating_Rate_Is_Less_Than_Zero()
    {
        var command = new UpdateProductCommand
        {
            Rating = Rating.Create(-1, 0)
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Rating.Rate);
    }

    [Fact(DisplayName = "Given invalid rating count Then returns validation error")]
    public void Should_Have_Error_When_Rating_Count_Is_Less_Than_Zero()
    {
        var command = new UpdateProductCommand
        {
            Rating = Rating.Create(0, -1)
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Rating.Count);
    }
}