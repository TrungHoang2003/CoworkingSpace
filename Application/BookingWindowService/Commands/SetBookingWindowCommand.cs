using Application.BookingWindowService.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.BookingWindowService.Commands;

public sealed record SetBookingWindowCommand(SetBookingWindowRequest SetBookingWindowRequest) : IRequest<Result>;

public class SetBookingWindowCommandHandler(IUnitOfWork unitOfWork, IMapper mapper): IRequestHandler<SetBookingWindowCommand, Result>
{
    public async Task<Result> Handle(SetBookingWindowCommand command, CancellationToken cancellationToken)
    {
        //Validate du lieu tu request
        command.SetBookingWindowRequest.Validate();
        
        //Cheking xem venue co ton tai khong
       var venue = await unitOfWork.Venue.GetById(command.SetBookingWindowRequest.VenueId);
       if (venue == null)
           return VenueErrors.VenueNotFound;
       
       //Tao moi booking window
       var bookingWindow = mapper.Map<BookingWindow>(command.SetBookingWindowRequest);
       await unitOfWork.BookingWindow.Create(bookingWindow);
       
       //Them booking window cho cac space
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