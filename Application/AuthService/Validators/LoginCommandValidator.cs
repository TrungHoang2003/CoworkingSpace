using Application.AuthService.CQRS.Commands;
using FluentValidation;

namespace Application.AuthService.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
