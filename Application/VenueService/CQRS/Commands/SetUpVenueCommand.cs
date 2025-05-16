using Application.GuestHourService.DTOs;
using Application.GuestHourService.Mappings;
using Application.VenueAddressService.DTOs;
using Application.VenueAddressService.Mappings;
using Application.VenueService.DTOs;
using Application.VenueService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.VenueService.CQRS.Commands;

public sealed record SetUpVenueCommand(
    int VenueId,
    SetUpVenueDetailsDto? Details,
    GuestArrivalDto? GuestArrival,
    SetUpVenueAddressDto? Address,
    List<SetUpVenueGuestHourDto>? GuestHours,
    List<int>? HolidayIds
) : IRequest<Result>;

public class SetUpVenueCommandHandler(IUnitOfWork unitOfWork)
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
            venue.Floor = request.Details.Floor;
        }

        if (request.Address is not null)
        {
            var address = request.Address.ToVenueAddress();
            venue.Address = address;
        }
        
        // Update GuestHours
        if (request.GuestHours is { Count: > 0 })
        {
            var existingGuestHours =
                await unitOfWork.GuestHour.GetGuestHoursByVenueId(request.VenueId);

            // Xóa GuestHours hiện tại của Venue
            if (existingGuestHours.Count > 0)
                unitOfWork.GuestHour.RemoveRange(existingGuestHours);

            foreach (var guestHourDto in request.GuestHours)
            {
                var guestHour = guestHourDto.ToGuestHour();
                venue.GuestHours.Add(guestHour);
            }
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