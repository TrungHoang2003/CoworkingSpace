using Application.BookingWindowService.DTOs;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.BookingWindowService.Commands;

public sealed record SetBookingWindowCommand(SetBookingWindowRequest SetBookingWindowRequest) : IRequest<Result>;

public class SetBookingWindowCommandHandler(IUnitOfWork unitOfWork): IRequestHandler<SetBookingWindowCommand, Result>
{
    public async Task<Result> Handle(SetBookingWindowCommand command, CancellationToken cancellationToken)
    {
        command.SetBookingWindowRequest.Validate();
        
       var venue = await unitOfWork.Venue.GetById(command.SetBookingWindowRequest.VenueId);
       if (venue == null)
           return VenueErrors.VenueNotFound;

       var bookingWindow = new BookingWindow
       {
            MaxNoticeDays = command.SetBookingWindowRequest.MaxNoticeDays,
            MinNotice = command.SetBookingWindowRequest.MinNotice,
            Unit = command.SetBookingWindowRequest.Unit,
       };
       await unitOfWork.BookingWindow.Create(bookingWindow);
       
       foreach (var spaceId in command.SetBookingWindowRequest.SpaceIds)
       {
           var space = await unitOfWork.Space.GetById(spaceId);
           if (space == null)
               return SpaceErrors.SpaceNotFound;
           
           if (space.VenueId != venue.VenueId)
               return SpaceErrors.SpaceNotFoundInVenue;
        
           space.BookingWindow = bookingWindow;
           await unitOfWork.Space.Update(space);
       }

       await unitOfWork.SaveChangesAsync(cancellationToken);
       return Result.Success();
    }
}