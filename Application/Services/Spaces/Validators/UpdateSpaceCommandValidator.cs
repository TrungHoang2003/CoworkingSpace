using Application.Services.Spaces.CQRS.Commands;
using Application.SpaceService.CQRS.Commands;
using Domain.Entities;
using FluentValidation;

namespace Application.Services.Spaces.Validators;

public class UpdateSpaceCommandValidator : AbstractValidator<UpdateSpace>
{
    public UpdateSpaceCommandValidator()
    {
        RuleFor(x => x.BasicInfo.SpaceTypeId)
            .GreaterThan(0)
            .WithMessage("SpaceTypeId must be greater than 0.")
            .When(x => x.BasicInfo != null);

        RuleFor(x => x.Price.Amount)
            .GreaterThan(0)
            .WithMessage("Space price amount must be greater than 0.");

        RuleFor(x => x.AmenityIds)
            .NotNull()
            .When(x => x.AmenityIds != null && x.AmenityIds.Count != 0)
            .WithMessage("At least one amenity must be selected.");
    }
}