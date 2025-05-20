using Application.AuthService.CQRS.Commands;
using Application.Services.Auths.CQRS.Commands;
using FluentValidation;

namespace Application.AuthService.Validators;

public class GoogleRegisterCommandValidator : AbstractValidator<GoogleRegisterCommand>
{
    public GoogleRegisterCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.AvatarUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.AvatarUrl))
            .WithMessage("Avatar URL must be a valid URL.");
    }
}
