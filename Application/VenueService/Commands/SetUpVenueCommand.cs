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
            mapper.Map(request.Address, venue);
            await unitOfWork.Venue.Update(venue);
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

        // Set Exception
        if (request.Exception != null)
        {
            request.Exception.Validate();

            var exceptionRule = mapper.Map<ExceptionRule>(request);
            await unitOfWork.Exception.Create(exceptionRule);

            if (request.Exception.ApplyAll)
            {
                var spaces = await unitOfWork.Space.GetVenueWorkingSpacesAsync(request.VenueId);
                foreach (var space in spaces)
                {
                    space.Exception = exceptionRule;
                    await unitOfWork.Space.Update(space);
                }
            }

            foreach (var spaceId in request.Exception.SpaceIds!)
            {
                var space = await unitOfWork.Space.GetByIdAndVenue(spaceId, request.VenueId);
                if (space == null)
                    return Result.Failure(new Error("Space Error", "Cannot find space with Id = " + spaceId + ""));

                space.Exception = exceptionRule;
                await unitOfWork.Space.Update(space);
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

        // Set Booking Window
        if (request.BookingWindow != null)
        {
            //Validate du lieu tu request
            request.BookingWindow.Validate();

            //Tao moi booking window
            var bookingWindow = mapper.Map<BookingWindow>(request.BookingWindow);
            await unitOfWork.BookingWindow.Create(bookingWindow);

            //Them booking window cho cac space
            if (request.BookingWindow.ApplyAll)
            {
                var spaces = await unitOfWork.Space.GetVenueWorkingSpacesAsync(request.VenueId);
                foreach (var space in spaces)
                {
                    space.BookingWindow = bookingWindow;
                    await unitOfWork.Space.Update(space);
                }
            }
            foreach (var spaceId in request.BookingWindow.SpaceIds!)
            {
                var space = await unitOfWork.Space.GetByIdAndVenue(spaceId, request.VenueId);
                if (space == null)
                    return Result.Failure(new Error("Space Error", "Cannot find space with Id = " + spaceId + ""));

                space.BookingWindow = bookingWindow;
                await unitOfWork.Space.Update(space);
            }
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}