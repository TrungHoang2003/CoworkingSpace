using Application.SpaceService.CQRS.Commands;
using Domain.Entities;
using FluentValidation;

namespace Application.SpaceService.Validators;

public class CreateSpaceCommandValidator : AbstractValidator<CreateSpaceCommand>
{
    public CreateSpaceCommandValidator()
    {
        RuleFor(x => x.VenueId)
            .GreaterThan(0)
            .WithMessage("VenueId must be greater than 0.");

        RuleFor(x => x.SpaceInfos)
            .NotNull()
            .WithMessage("Space information must be provided.");

        RuleFor(x => x.SpaceInfos.SpaceTypeId)
            .GreaterThan(0)
            .WithMessage("SpaceTypeId must be greater than 0.");

        RuleFor(x => x.SpaceInfos.ListingType)
            .NotNull()
            .WithMessage("ListingType must be set.");

        RuleFor(x => x.SpaceInfos.ListingType)
            .IsInEnum()
            .WithMessage("ListingType must be either Daily(1) or Monthly(0).");

        RuleFor(x => x.SpaceInfos.Capacity)
            .Null()
            .When(x => x.SpaceInfos.ListingType == ListingType.Daily)
            .WithMessage("If you are creating a daily space, capacity is not required");

        RuleFor(x => x.SpaceInfos.Quantity)
            .Null()
            .When(x => x.SpaceInfos.ListingType == ListingType.Monthly)
            .WithMessage("If you are creating a monthly space, quantity is not required");

        RuleFor(x => x.SpaceInfos.Name)
            .NotEmpty()
            .WithMessage("Space name cannot be empty.");

        RuleFor(x => x.SpaceInfos.Description)
            .NotEmpty()
            .WithMessage("Space description cannot be empty.");

        RuleFor(x => x.SpacePrice)
            .NotNull()
            .WithMessage("Space price information must be provided.");

        RuleFor(x => x.SpacePrice.Amount)
            .GreaterThan(0)
            .WithMessage("Space price amount must be greater than 0.");

        RuleFor(x => x.SpacePrice.DiscountPercentage)
            .Null()
            .When(x => x.SpaceInfos.ListingType == ListingType.Daily)
            .WithMessage("Daily spaces do not have discount percentage.");

        RuleFor(x => x.SpacePrice.SetupFee)
            .Null()
            .When(x => x.SpaceInfos.ListingType == ListingType.Daily)
            .WithMessage("Daily spaces do not have a setup fee.");

        RuleFor(x => x.SpacePrice.DiscountPercentage)
            .InclusiveBetween(0, 100)
            .When(x => x.SpaceInfos.ListingType == ListingType.Monthly)
            .WithMessage("Discount percentage must be between 0 and 100.");

        RuleFor(x => x.SpacePrice.SetupFee)
            .GreaterThanOrEqualTo(0)
            .When(x => x.SpaceInfos.ListingType == ListingType.Monthly)
            .WithMessage("Setup fee must be greater than or equal to 0.");

        RuleFor(x => x.AmenityIds)
            .NotNull()
            .When(x => x.AmenityIds != null && x.AmenityIds.Count != 0)
            .WithMessage("At least one amenity must be selected.");
    }
}