using Application.BookingWindowService.CQRS.Commands;
using Domain.Entities;
using FluentValidation;

namespace Application.BookingWindowService.Validators;

public class UpdateBookingWindowCommandValidator: AbstractValidator<UpdateBookingWindowCommand>
{
   public UpdateBookingWindowCommandValidator()
   {
      RuleFor(x => x.BookingWindowId).GreaterThan(0).NotNull();
      RuleFor(x => x.VenueId).GreaterThan(0).NotNull();
      RuleFor(x => x.MinNotice).GreaterThanOrEqualTo(0);
      RuleFor(x => x.Unit).NotNull();
      RuleFor(x => x.MaxNoticeDays)
         .NotNull()
         .When(x => x.Unit == BookingTimeUnit.Days)
         .WithMessage("Max notice days must be provided when unit is days.");
      RuleFor(x => x.SpaceIds)
         .NotEmpty()
         .When(x => !x.ApplyAll)
         .WithMessage("At least one space is required when ApplyAll is false.");
   } 
}