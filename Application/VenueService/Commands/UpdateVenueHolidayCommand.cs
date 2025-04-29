using Application.HolidayService.DTOs;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.VenueService.Commands;

public sealed record UpdateVenueHolidayCommand(UpdateHolidayRequest UpdateHolidayRequest): IRequest<Result>;

public class UpdateVenueHolidayCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateVenueHolidayCommand, Result>
{
    public async Task<Result> Handle(UpdateVenueHolidayCommand command, CancellationToken cancellationToken)
    {
        //lấy ra danh sách Holiday dựa trên danh sách Id gửi từ client
        foreach (var holidayId in command.UpdateHolidayRequest.HolidayIds)
        {
            var holiday = await unitOfWork.Holiday.GetById(holidayId);
            if (holiday == null)
                return Result.Failure(new Error("Holiday Error", "Cannot find holiday with Id = " + holidayId + ""));
            
            // Lấy ra venueHoliday và cập nhật observed thành true
            var venueHoliday = await unitOfWork.VenueHoliday.GetByVenueIdAndHolidayId(holidayId, command.UpdateHolidayRequest.VenueId);
            if (venueHoliday == null)
                return Result.Failure(new Error("VenueHoliday Error", "Cannot find venueHoliday with Id = " + holidayId + ""));
            
            venueHoliday.IsObserved = true;
            await unitOfWork.VenueHoliday.Update(venueHoliday);
        } 
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}