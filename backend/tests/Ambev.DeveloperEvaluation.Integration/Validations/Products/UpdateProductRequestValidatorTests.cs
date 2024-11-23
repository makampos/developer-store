using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Validations.Products;


public class UpdateProductRequestValidatorTests
{
    private readonly UpdateProductRequestValidator _validator;
    private readonly Guid _validId = Guid.NewGuid();

    public UpdateProductRequestValidatorTests()
    {
        _validator = new UpdateProductRequestValidator(_validId);
    }

    [Fact(DisplayName = "Should have error when Id is empty")]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var request = new UpdateProductRequest { Id = Guid.Empty };
        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Product ID is required");
    }

    [Fact(DisplayName = "Should have error when Id does not match route parameter")]
    public void Should_Have_Error_When_Id_Does_Not_Match_Route_Parameter()
    {
        var request = new UpdateProductRequest { Id = Guid.NewGuid() };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Product ID must match route parameter");
    }

    [Fact(DisplayName = "Should have error when Title is empty")]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var request = new UpdateProductRequest { Title = string.Empty };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required");
    }

    [Fact(DisplayName = "Should have error when Description is empty")]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var request = new UpdateProductRequest { Description = string.Empty };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description is required");
    }

    [Fact(DisplayName = "Should have error when Category is empty")]
    public void Should_Have_Error_When_Category_Is_Empty()
    {
        var request = new UpdateProductRequest { Category = string.Empty };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Category)
            .WithErrorMessage("Category is required");
    }

    [Fact(DisplayName = "Should have error when Image is empty")]
    public void Should_Have_Error_When_Image_Is_Empty()
    {
        var request = new UpdateProductRequest { Image = string.Empty };
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Image)
            .WithErrorMessage("Image is required");
    }

    [Fact(DisplayName = "Should have error when Rating Rate is less than zero")]
    public void Should_Have_Error_When_Rating_Rate_Is_Less_Than_Zero()
    {
        var request = new UpdateProductRequest
        {
            Rating = Rating.Create(-1, 0)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Rating.Rate)
            .WithErrorMessage("Rating rate must be greater than or equal to 0");
    }

    [Fact(DisplayName = "Should have error when Rating Count is less than zero")]
    public void Should_Have_Error_When_Rating_Count_Is_Less_Than_Zero()
    {
        var request = new UpdateProductRequest
        {
            Rating = Rating.Create(0, -1)
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Rating.Count)
              .WithErrorMessage("Rating count must be greater than or equal to 0");
    }
}
