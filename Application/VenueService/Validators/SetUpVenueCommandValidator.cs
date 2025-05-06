using Application.DTOs;
using Application.GuestHourService.Validators;
using Application.VenueService.CQRS.Commands;
using Application.VenueService.DTOs;
using FluentValidation;

namespace Application.VenueService.Validators;

public class SetUpVenueCommandValidator: AbstractValidator<SetUpVenueCommand>
{
   public SetUpVenueCommandValidator()
   {
      RuleFor(x => x.VenueId)
         .GreaterThan(0)
         .WithMessage("VenueId must be greater than 0.");

      RuleFor(x => x.GuestHours)
         .Must(guestHours => guestHours == null || guestHours.Count == 0 || (guestHours.Count == 7 && guestHours.Select(x => x.DayOfWeek).Distinct().Count() == 7))
         .When(x => x.GuestHours != null)
         .WithMessage("Guest hours must be provided for all 7 days of the week.");

      RuleForEach(x => x.GuestHours)
         .SetValidator(new GuestHourDtoValidator());

      RuleFor(x => x.HolidayIds)
         .Must(holidayIds => holidayIds == null || holidayIds.Count > 0)
         .When(x => x.HolidayIds != null)
         .WithMessage("HolidayIds must not be empty if provided.");
   }
}