using Application.GuestHourService.DTOs;
using Application.GuestHourService.Mappings;
using Application.VenueAddressService.DTOs;
using Application.VenueService.DTOs;
using Application.VenueService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using FluentValidation;
using Infrastructure.Repositories;
using MediatR;

namespace Application.VenueService.CQRS.Commands;

public sealed record SetUpVenueCommand(
    int VenueId,
    SetUpVenueDetailsDto? Details,
    SetUpVenueAddressDto? Address,
    List<SetUpVenueGuestHourDto>? GuestHours,
    List<int>? HolidayIds
) : IRequest<Result>;

public class SetUpVenueCommandHandler(IUnitOfWork unitOfWork, IValidator<SetUpVenueCommand> validator)
    : IRequestHandler<SetUpVenueCommand, Result>
{
    public async Task<Result> Handle(SetUpVenueCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if(!validatorResult.IsValid)
            return Result.Failure(new Error("Validation Errors", string.Join("\n",validatorResult.Errors.Select(x => x.ErrorMessage).ToList())));
        
        var venue = await unitOfWork.Venue.GetById(request.VenueId);
        if (venue == null) return VenueErrors.VenueNotFound;
        
        // Update Venue 
        venue = request.ToVenue(venue);
        await unitOfWork.Venue.Update(venue);
        
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
                var newGuestHours = new List<GuestHour>();
                var guestHour = guestHourDto.ToGuestHour();
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