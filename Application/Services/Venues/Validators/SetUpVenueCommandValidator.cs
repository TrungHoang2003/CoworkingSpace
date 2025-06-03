using Application.Services.GuestHours.Validators;
using Application.Services.Venues.CQRS.Commands;
using FluentValidation;

namespace Application.Services.Venues.Validators;

public class SetUpVenueCommandValidator : AbstractValidator<SetUpVenueCommand>
{
   public SetUpVenueCommandValidator()
   {
      RuleFor(x => x.VenueId)
         .GreaterThan(0)
         .WithMessage("VenueId must be greater than 0.");

      RuleFor(x => x.GuestHours)
         .SetValidator(new SetUpVenueGuestHourDtoValidator())
         .When(x => x.GuestHours != null);

      RuleFor(x => x.HolidayIds)
         .Must(holidayIds => holidayIds == null || holidayIds.Count > 0)
         .When(x => x.HolidayIds != null)
         .WithMessage("HolidayIds must not be empty if provided.");
   }
}