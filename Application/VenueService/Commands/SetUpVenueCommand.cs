using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.VenueService.Commands;

public sealed record SetUpVenueCommand(SetUpVenueRequest SetUpVenueRequest):IRequest<Result>;

public class SetUpVenueCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<SetUpVenueCommand, Result>
{
    public async Task<Result> Handle(SetUpVenueCommand command, CancellationToken cancellationToken)
    {
        var request = command.SetUpVenueRequest;
        var venue = await unitOfWork.Venue.GetById(request.VenueId);
        if (venue == null) return VenueErrors.VenueNotFound;

        // Update Venue Basic Informations
        if (request.Details != null)
        {
            mapper.Map(request.Details, venue);
            await unitOfWork.Venue.Update(venue);
        }

        // Update Venue Address
        if (request.Address != null)
        {
            // Check if Venue has address, if not create address and update Venue, if yes update Venue's address
            var venueAddress = await unitOfWork.VenueAddress.GetById(venue.VenueAddressId);
            if(venueAddress is null)
            {
                var newVenueAddress = mapper.Map<VenueAddress>(request.Address);
                await unitOfWork.VenueAddress.Create(newVenueAddress);
                venue.Address = newVenueAddress;
                await unitOfWork.Venue.Update(venue);
            }
            else
            {
                mapper.Map(request.Address, venueAddress);
                await unitOfWork.VenueAddress.Update(venueAddress);
            }
        }

        // Update GuestHours
        if (request.GuestHours is { Count: > 0 })
        {
            // Validate Request (Đảm bảo chỉ có 7 ngày trong tuần)
            request.GuestHoursValidate();

            var existingGuestHours =
                await unitOfWork.GuestHour.GetGuestHoursByVenueId(request.VenueId);

            // Xóa GuestHours hiện tại của Venue
            if (existingGuestHours.Count > 0)
                unitOfWork.GuestHour.RemoveRange(existingGuestHours);

            foreach (var guestHourDto in request.GuestHours)
            {
                var newGuestHours = new List<GuestHour>();
                var guestHour = mapper.Map<GuestHour>(guestHourDto);
                newGuestHours.Add(guestHour);
                await unitOfWork.GuestHour.AddRangeAsync(newGuestHours);
            }
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
                await unitOfWork.VenueHoliday.Update(venueHoliday);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}