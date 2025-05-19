using Application.Services.Spaces.CQRS.Commands;
using Application.SpaceService.CQRS.Commands;
using Domain.Entities;
using FluentValidation;

namespace Application.SpaceService.Validators;

public class AddSpaceImageCommandValidator:AbstractValidator<AddSpaceImageCommand>
{
    public AddSpaceImageCommandValidator()
    {
        RuleFor(x => x.Image)
            .NotNull()
            .WithMessage("Image is required");
        
        RuleFor(x => x.SpaceId)
            .GreaterThan(0)
            .WithMessage("SpaceId must be greater than 0");
        
        RuleFor(x => x.Type)
            .IsInEnum()
            .Must(x => x is SpaceAssetType.Workspace or SpaceAssetType.CommonArea)
            .WithMessage("Image type must be either Workspace(0) or CommonArea(1)");
    }
}