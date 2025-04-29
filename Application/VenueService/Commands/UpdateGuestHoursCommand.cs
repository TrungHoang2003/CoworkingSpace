using Application.GuestHoursService.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.VenueService.Commands;

public sealed record UpdateGuestHoursCommand(UpdateGuestHoursRequest UpdateGuestHoursRequest) : IRequest<Result>;

public class UpdateGuestHoursCommandHandler(IMapper mapper, IUnitOfWork unitOfWork): IRequestHandler<UpdateGuestHoursCommand, Result>
{
    public async Task<Result> Handle(UpdateGuestHoursCommand command, CancellationToken cancellationToken)
    {
        // Validate Request (Đảm bảo chỉ có 7 ngày trong tuần)
        command.UpdateGuestHoursRequest.Validate();
        
        var venue = await unitOfWork.Venue.GetById(command.UpdateGuestHoursRequest.VenueId);
        if (venue == null)
            return VenueErrors.VenueNotFound;
        
        var existingGuestHours = await unitOfWork.GuestHour.GetGuestHoursByVenueId(command.UpdateGuestHoursRequest.VenueId);
        
        // Xóa GuestHours hiện tại của Venue
        if(existingGuestHours.Count>0)
            unitOfWork.GuestHour.RemoveRange(existingGuestHours);

        foreach (var guestHourDto in command.UpdateGuestHoursRequest.GuestHours)
        {
            var newGuestHours = new List<GuestHour>();
            var guestHour = new GuestHour
            {
                VenueId = venue.VenueId,
                DayOfWeek = guestHourDto.DayOfWeek,
                StartTime = guestHourDto.StartTime,
                EndTime = guestHourDto.EndTime,
                IsOpen24Hours = guestHourDto.IsOpen24Hours,
                IsClosed = guestHourDto.IsClosed
            };
            newGuestHours.Add(guestHour);
            await unitOfWork.GuestHour.AddRangeAsync(newGuestHours);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        
        return Result.Success();
    }
}