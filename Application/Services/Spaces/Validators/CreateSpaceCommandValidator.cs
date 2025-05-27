using Application.Services.Spaces.CQRS.Commands;
using Domain.Entities;
using FluentValidation;

namespace Application.Services.Spaces.Validators;

public class CreateSpaceCommandValidator : AbstractValidator<CreateSpace>
{
    public CreateSpaceCommandValidator()
    {
        RuleFor(x => x.VenueId)
            .GreaterThan(0)
            .WithMessage("VenueId must be greater than 0.");

        RuleFor(x => x.BasicInfo)
            .NotNull()
            .WithMessage("Space information must be provided.");

        RuleFor(x => x.BasicInfo.SpaceTypeId)
            .GreaterThan(0)
            .WithMessage("SpaceTypeId must be greater than 0.");

        RuleFor(x => x.BasicInfo.ListingType)
            .NotNull()
            .WithMessage("ListingType must be set.");

        RuleFor(x => x.BasicInfo.ListingType)
            .IsInEnum()
            .WithMessage("ListingType must be either MonthOnly(0) or Normal(1).");

        RuleFor(x => x.BasicInfo.Name)
            .NotEmpty()
            .WithMessage("Space name cannot be empty.");

        RuleFor(x => x.BasicInfo.Description)
            .NotEmpty()
            .WithMessage("Space description cannot be empty.");

        RuleFor(x => x.Price)
            .NotNull()
            .WithMessage("Space price information must be provided.");

        RuleFor(x => x.Price.Amount)
            .GreaterThan(0)
            .WithMessage("Space price amount must be greater than 0.");

        RuleFor(x => x.AmenityIds)
            .NotNull()
            .When(x => x.AmenityIds != null && x.AmenityIds.Count != 0)
            .WithMessage("At least one amenity must be selected.");
    }
}