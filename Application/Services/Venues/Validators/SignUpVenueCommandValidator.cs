using Application.Services.Venues.CQRS.Commands;
using Application.VenueService.CQRS.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.VenueService.Validators;

public class SignUpVenueCommandValidator: AbstractValidator<SignUpVenueCommand>
{
   public SignUpVenueCommandValidator() 
   {
      RuleFor(x=>x.VenueTypeId)
         .NotNull()
         .GreaterThan(0)
         .WithMessage("VenueTypeId is required.");
      
      RuleFor(x => x.Address)
         .NotNull()
         .WithMessage("Address is required.");
      
      RuleFor(x => x.Address.Street)
         .NotNull()
         .WithMessage("Street is required.");
      
      RuleFor(x => x.Address.District)
         .NotNull()
         .WithMessage("District is required.");
      
      RuleFor(x => x.Address.City)
         .NotNull()
         .WithMessage("City is required.");
      
      RuleFor(x=>x.PhoneNumber)
         .NotNull()
         .WithMessage("Phone Number is required.");
      
      RuleFor(x => x.Description)
         .NotNull()
         .WithMessage("Description is required.");
      
      RuleFor(x => x.Name)
         .NotNull()
         .WithMessage("Venue Name is required.");
   }
}