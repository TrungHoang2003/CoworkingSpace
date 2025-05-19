using Application.VenueService.DTOs;
using Domain.Entities;
using FluentValidation;

namespace Application.VenueService.Validators;

public class GuestArrivalValidator: AbstractValidator<GuestArrivalDto>
{
    public GuestArrivalValidator()
    {
        RuleFor(x=>x.ParkingType)
            .IsInEnum()
            .WithMessage("Parking type must be either FreeParkingOnsite(0) or PaidParkingOnsite(2) or FreeParkingOffsite(1) or PaidParkingOffsite(3).");
        RuleFor(x => x.WelcomeMessage)
            .MaximumLength(500)
            .WithMessage("Welcome message must not exceed 500 characters.");

        RuleFor(x => x.EntryInformation)
            .MaximumLength(500)
            .WithMessage("Entry information must not exceed 500 characters.");

        RuleFor(x => x.ParkingInformation)
            .MaximumLength(500)
            .WithMessage("Parking information must not exceed 500 characters.");

        RuleFor(x => x.ParkingPrice)
            .NotNull()
            .When(x => x.ParkingType is ParkingType.PaidParkingOnsite or ParkingType.PaidParkingOffsite)
            .WithMessage("Giá đỗ xe không được để trống khi loại đỗ xe là có phí.");
        
        RuleFor(x => x.ParkingPrice)
            .Null()
            .When(x => x.ParkingType is ParkingType.FreeParkingOnsite or ParkingType.FreeParkingOffsite)
            .WithMessage("Giá đỗ xe là không cần thiết khi loại đỗ xe là miễn phí.");

        RuleFor(x => x.WifiName)
            .MaximumLength(100)
            .WithMessage("WiFi name must not exceed 100 characters.");

        RuleFor(x => x.WifiPassword)
            .MaximumLength(100)
            .WithMessage("WiFi password must not exceed 100 characters.");
    }
}