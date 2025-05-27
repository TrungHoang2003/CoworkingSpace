using Application.Services.GuestHours.DTOs;
using Application.Services.GuestHours.Mappings;
using Application.Services.Venues.DTOs;
using Application.SharedServices;
using Application.VenueAddressService.DTOs;
using Application.VenueAddressService.Mappings;
using Application.VenueService.DTOs;
using Application.VenueService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Venues.CQRS.Commands;

public sealed record SetUpVenueCommand(
    int VenueId,
    SetUpVenueDetailsDto? Details,
    GuestArrivalDto? GuestArrival,
    SetUpVenueAddressDto? Address,
    SetUpVenueGuestHourDto? GuestHours,
    List<int>? HolidayIds
) : IRequest<Result>;

public class SetUpVenueCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService)
    : IRequestHandler<SetUpVenueCommand, Result>
{
    public async Task<Result> Handle(SetUpVenueCommand request, CancellationToken cancellationToken)
    {
        var venue = await unitOfWork.Venue.GetById(request.VenueId);
        if (venue == null) return VenueErrors.VenueNotFound;
        
        // Update Venue 
        if (request.Details is not null)
        {
            venue.Name = request.Details.Name;
            venue.Description = request.Details.Description;
            if(request.Details.Logo is not null)
            {
                var logoUrl = await cloudinaryService.UploadImage(request.Details.Logo);
                if (logoUrl == null) return CloudinaryErrors.UploadVenueLogoFailed;
                venue.LogoUrl = logoUrl;
            }
        }

        if (request.Address is not null)
        {
            var address = request.Address.ToVenueAddress();
            venue.Address = address;
        }
        
        // Update GuestHours
        if (request.GuestHours is not null)
        {
            var guestHours = request.GuestHours.ToGuestHour();
            venue.GuestHour = guestHours;
        }
        
        // Update GuestArrival
        if (request.GuestArrival != null)
        {
            var guestArrival = request.GuestArrival.ToGuestArrival();
            if (venue.GuestArrival is null)
            {
                await unitOfWork.GuestArrival.Create(guestArrival);
                venue.GuestArrival = guestArrival;
            }
            venue.GuestArrival = guestArrival;
        }
        
        // Set Observed Holidays
        if (request.HolidayIds is { Count: > 0 })
        {
            //lấy ra danh sách Holiday dựa trên danh sách Id gửi từ client
            foreach (var holidayId in request.HolidayIds)
            {
                var holiday = await unitOfWork.Holiday.GetById(holidayId);
                if (holiday == null)
                    return Result.Failure(new Error("Holiday Error",
                        "Cannot find holiday with Id = " + holidayId + ""));

                // Lấy ra venueHoliday và cập nhật observed thành true
                var venueHoliday =
                    await unitOfWork.VenueHoliday.GetByVenueIdAndHolidayId(holidayId,
                        request.VenueId);
                if (venueHoliday == null)
                    return Result.Failure(new Error("VenueHoliday Error",
                        "Cannot find venueHoliday with Id = " + holidayId + ""));

                venueHoliday.IsObserved = true;
                (venue.Holidays ??= new List<VenueHoliday>()).Add(venueHoliday);
            }
        }
        await unitOfWork.Venue.Update(venue);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}