using Application.AuthService.CQRS.Commands;
using FluentValidation;

namespace Application.AuthService.Validators;

public class GoogleCallbackCommandValidator : AbstractValidator<GoogleCallbackCommand>
{
    public GoogleCallbackCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.");
    }
}
