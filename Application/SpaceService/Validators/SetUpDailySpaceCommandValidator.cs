using Application.PriceService.DTOs;
using Application.SpaceService.CQRS.Commands;
using Application.SpaceService.DTOs;
using FluentValidation;

namespace Application.SpaceService.Validators;

public class BasicSpaceInfosValidator : AbstractValidator<SpaceInfos>
{
    public BasicSpaceInfosValidator()
    {
        RuleFor(x => x.SpaceTypeId)
            .GreaterThan(0).WithMessage("SpaceTypeId must be greater than 0.");
    }
}

public class DailySpacePriceDtoValidator : AbstractValidator<DailySpacePriceDto>
{
    public DailySpacePriceDtoValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0.");
    }
}