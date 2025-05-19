using Application.Services.Exceptions.CQRS.Commands;
using FluentValidation;

namespace Application.Services.Exceptions.Validators;

public class UpdateExceptionCommandValidator: AbstractValidator<UpdateExceptionCommand>
{
   public UpdateExceptionCommandValidator()
   {
      // Quy tắc cho ExceptionId
      RuleFor(x => x.ExceptionId)
         .GreaterThan(0)
         .WithMessage("ExceptionId must be greater than 0.");

      // Quy tắc cho VenueId
      RuleFor(x => x.VenueId)
         .GreaterThan(0)
         .WithMessage("VenueId must be greater than 0.");

      // Quy tắc cho ApplyAll và SpaceIds
      RuleFor(x => x.SpaceIds)
         .NotNull()
         .When(x => !x.ApplyAll)
         .WithMessage("SpaceIds must be provided when ApplyAll is false.");

      RuleFor(x => x.SpaceIds)
         .Must(spaceIds => spaceIds is { Count: > 0 })
         .When(x => !x.ApplyAll)
         .WithMessage("At least one space is required when ApplyAll is false.");

      RuleFor(x => x.StartTime)
         .Null()
         .When(x => x.IsClosed24Hours)
         .WithMessage("Start Time doesnt required when IsClosed24Hours is true.");

      RuleFor(x => x.EndTime)
         .Null()
         .When(x => x.IsClosed24Hours)
         .WithMessage("End Time doesnt required when IsClosed24Hours is true.");
   }
}