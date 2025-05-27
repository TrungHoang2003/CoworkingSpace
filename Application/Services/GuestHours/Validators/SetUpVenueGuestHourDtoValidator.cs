using System.Data;
using Application.Services.GuestHours.DTOs;
using FluentValidation;

namespace Application.Services.GuestHours.Validators;

public class SetUpVenueGuestHourDtoValidator: AbstractValidator<SetUpVenueGuestHourDto>
{
    public SetUpVenueGuestHourDtoValidator()
    {
        RuleFor(x=>x.OpenOnSaturday)
            .NotNull()
            .WithMessage("OpenOnSaturday is required.");

        RuleFor(x => x.IsClosed)
            .Must(x=>x == false)
            .When(x => x.Open24Hours)
            .WithMessage("IsClosed must be false if open24Hours.");
        
        RuleFor(x => x.Open24Hours)
            .Must(x=>x == false)
            .When(x => x.IsClosed)
            .WithMessage("Open24Hours must be false if IsClosed.");
        
        RuleFor(x => x.StartTime)
            .Null()
            .When(x => x.IsClosed || x.Open24Hours)
            .WithMessage("StartTime must be null when the venue is closed or open 24 hours.")
            .NotNull()
            .When(x => !x.IsClosed && !x.Open24Hours)
            .WithMessage("StartTime is required when the venue is neither closed nor open 24 hours.");

        RuleFor(x => x.EndTime)
            .Null()
            .When(x => x.IsClosed || x.Open24Hours)
            .WithMessage("EndTime must be null when the venue is closed or open 24 hours.")
            .NotNull()
            .When(x => !x.IsClosed && !x.Open24Hours)
            .WithMessage("EndTime is required when the venue is neither closed nor open 24 hours."); 
    }    
}