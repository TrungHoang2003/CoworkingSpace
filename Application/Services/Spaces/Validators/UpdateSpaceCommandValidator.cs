using Application.Services.Spaces.CQRS.Commands;
using Application.SpaceService.CQRS.Commands;
using Domain.Entities;
using FluentValidation;

namespace Application.Services.Spaces.Validators;

public class UpdateSpaceCommandValidator : AbstractValidator<UpdateSpaceCommand>
{
    public UpdateSpaceCommandValidator()
    {
        RuleFor(x => x.BasicInfo.SpaceTypeId)
            .GreaterThan(0)
            .WithMessage("SpaceTypeId must be greater than 0.")
            .When(x => x.BasicInfo != null);

        RuleFor(x => x.BasicInfo.ListingType)
            .IsInEnum()
            .WithMessage("ListingType must be either Daily(1) or Monthly(0).")
            .When(x => x.BasicInfo != null);

        RuleFor(x => x.BasicInfo.Capacity)
            .Null()
            .When(x => x.BasicInfo.ListingType == ListingType.Daily)
            .WithMessage("If you are switching to a daily space, capacity is not required")
            .When(x => x.BasicInfo != null);

        RuleFor(x => x.BasicInfo.Quantity)
            .Null()
            .When(x => x.BasicInfo.ListingType == ListingType.Monthly)
            .WithMessage("If you are switching to a monthly space, quantity is not required")
            .When(x => x.BasicInfo != null);

        RuleFor(x => x.Price.Amount)
            .GreaterThan(0)
            .WithMessage("Space price amount must be greater than 0.");

        RuleFor(x => x.Price.DiscountPercentage)
            .Null()
            .When(x => x.BasicInfo.ListingType == ListingType.Daily)
            .WithMessage("Daily spaces do not have discount percentage.");

        RuleFor(x => x.Price.SetupFee)
            .Null()
            .When(x => x.BasicInfo.ListingType == ListingType.Daily)
            .WithMessage("Daily spaces do not have a setup fee.");

        RuleFor(x => x.Price.DiscountPercentage)
            .InclusiveBetween(0, 100)
            .When(x => x.BasicInfo.ListingType == ListingType.Monthly)
            .WithMessage("Discount percentage must be between 0 and 100.");

        RuleFor(x => x.Price.SetupFee)
            .GreaterThanOrEqualTo(0)
            .When(x => x.BasicInfo.ListingType == ListingType.Monthly)
            .WithMessage("Setup fee must be greater than or equal to 0.");

        RuleFor(x => x.AmenityIds)
            .NotNull()
            .When(x => x.AmenityIds != null && x.AmenityIds.Count != 0)
            .WithMessage("At least one amenity must be selected.");
    }
}