using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Carts.DeleteCart;

public class DeleteCartRequestValidator : AbstractValidator<DeleteCartRequest>
{
    public DeleteCartRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .WithMessage("Id is required");
    }
}