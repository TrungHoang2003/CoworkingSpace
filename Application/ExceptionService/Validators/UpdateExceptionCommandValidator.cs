using Application.ExceptionService.CQRS.Commands;
using Domain.Entities;
using FluentValidation;

namespace Application.ExceptionService.Validators;

public class UpdateExceptionCommandValidator: AbstractValidator<UpdateExceptionCommand>
{
   public UpdateExceptionCommandValidator()
   {
      RuleFor(x => x.Unit).NotNull().WithMessage("Unit is required.");
      RuleFor(x => x.Days)
         .NotNull()
         .When(x => x.Unit == ExceptionUnit.DaysOfWeek)
         .Must(days => days is { Count: > 0 })
         .WithMessage("Days are required for DaysOfWeek unit.");
      
      // Quy tắc cho ExceptionId
      RuleFor(x => x.ExceptionId)
         .GreaterThan(0)
         .WithMessage("ExceptionId must be greater than 0.");

      // Quy tắc cho VenueId
      RuleFor(x => x.VenueId)
         .GreaterThan(0)
         .WithMessage("VenueId must be greater than 0.");

      // Quy tắc cho Unit (bắt buộc)
      RuleFor(x => x.Unit)
         .NotNull()
         .WithMessage("Unit is required.");

      // Quy tắc cho DateRange
      RuleFor(x => x.StartDate)
         .NotNull()
         .When(x => x.Unit == ExceptionUnit.DateRange)
         .WithMessage("StartDate is required for DateRange unit.");

      RuleFor(x => x.EndDate)
         .NotNull()
         .When(x => x.Unit == ExceptionUnit.DateRange)
         .WithMessage("EndDate is required for DateRange unit.");

      RuleFor(x => x.EndDate)
         .GreaterThan(x => x.StartDate)
         .When(x => x.Unit == ExceptionUnit.DateRange && x is { StartDate: not null, EndDate: not null })
         .WithMessage("EndDate must be greater than StartDate.");

      // Quy tắc cho ApplyAll và SpaceIds
      RuleFor(x => x.SpaceIds)
         .NotNull()
         .When(x => !x.ApplyAll)
         .WithMessage("SpaceIds must be provided when ApplyAll is false.");

      RuleFor(x => x.SpaceIds)
         .Must(spaceIds => spaceIds is { Count: > 0 })
         .When(x => !x.ApplyAll)
         .WithMessage("At least one space is required when ApplyAll is false."); 
   }
}