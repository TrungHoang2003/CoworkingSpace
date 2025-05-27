using Application.Services.Bookings.CQRS.Commands;
using FluentValidation;

namespace Application.Services.Bookings.Validators;

public class DailySpaceBookCommandValidator: AbstractValidator<Book>
{
   public DailySpaceBookCommandValidator()
   {
         RuleFor(x => x.SpaceId)
            .GreaterThan(0)
            .WithMessage("SpaceId must be greater than 0.");
   
         RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("StartDate is required.")
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("StartDate must be in the future.");
   
         RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("EndDate is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("EndDate must be greater than StartDate.");
         
         RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0.");
         
   }
}