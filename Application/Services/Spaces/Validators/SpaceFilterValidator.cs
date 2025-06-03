using Domain.Filters;
using FluentValidation;

namespace Application.Services.Spaces.Validators;

public class SpaceFilterValidator : AbstractValidator<SpaceFilter>
{
    public SpaceFilterValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Listing Type must be either (1)Normal or (0)MonthOnly.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start date must be before or equal to end date.")
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}