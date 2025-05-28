using Domain.Filters;
using FluentValidation;

namespace Application.Services.Spaces.Validators;

public class SpaceFilterValidator:AbstractValidator<SpaceFilter>
{
    public SpaceFilterValidator()
    {
        RuleFor(x => x.ListingType)
            .IsInEnum()
            .WithMessage("Listing Type must be either (1)Normal or (0)MonthOnly.")
            .When(x => x.ListingType.HasValue);

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0.")
            .When(x => x.Capacity.HasValue);

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start date must be before or equal to end date.")
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}