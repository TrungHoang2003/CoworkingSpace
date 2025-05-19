using Application.GuestHourService.DTOs;
using FluentValidation;

namespace Application.GuestHourService.Validators;

public class SetUpVenueGuestHourDtoValidator: AbstractValidator<SetUpVenueGuestHourDto>
{
    public SetUpVenueGuestHourDtoValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .NotNull()
            .WithMessage("Day of week is required.");

        RuleFor(x => x.StartTime)
            .Null()
            .When(x => x.IsClosed || x.IsOpen24Hours)
            .WithMessage("StartTime must be null when the venue is closed or open 24 hours.")
            .NotNull()
            .When(x => !x.IsClosed && !x.IsOpen24Hours)
            .WithMessage("StartTime is required when the venue is neither closed nor open 24 hours.");

        RuleFor(x => x.EndTime)
            .Null()
            .When(x => x.IsClosed || x.IsOpen24Hours)
            .WithMessage("EndTime must be null when the venue is closed or open 24 hours.")
            .NotNull()
            .When(x => !x.IsClosed && !x.IsOpen24Hours)
            .WithMessage("EndTime is required when the venue is neither closed nor open 24 hours."); 
    }    
}