using Application.DTOs;
using AutoMapper;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.BookingWindowService;

public sealed record UpdateBookingWindowCommand(UpdateBookingWindowRequest UpdateBookingWindowRequest): IRequest<Result>;

public class UpdateBookingWindowCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateBookingWindowCommand, Result>
{
    public async Task<Result> Handle(UpdateBookingWindowCommand command, CancellationToken cancellationToken)
    {
        var request = command.UpdateBookingWindowRequest;
        //Validate du lieu tu request
        request.Validate();

        var bookingWindow = await unitOfWork.BookingWindow.GetById(request.BookingWindowId);
        if (bookingWindow is null) return BookingWindowErrors.BookingWindowNotFound;
        
        //Update booking window
        mapper.Map(request, bookingWindow); 
        await unitOfWork.BookingWindow.Update(bookingWindow);
        
        //Them booking window cho cac space
        if (request.ApplyAll)
        {
            var spaces = await unitOfWork.Space.GetVenueWorkingSpacesAsync(request.VenueId);
            foreach (var space in spaces)
            {
                space.BookingWindow = bookingWindow;
                await unitOfWork.Space.Update(space);
            }
        }

        foreach (var spaceId in request.SpaceIds!)
        {
            var space = await unitOfWork.Space.GetByIdAndVenue(spaceId, request.VenueId);
            if (space == null)
                return Result.Failure(new Error("Space Error", "Cannot find space with Id = " + spaceId + ""));

            space.BookingWindow = bookingWindow;
            await unitOfWork.Space.Update(space);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}