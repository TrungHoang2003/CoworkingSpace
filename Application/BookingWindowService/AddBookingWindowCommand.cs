using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.BookingWindowService;

public sealed record AddBookingWindowCommand(AddBookingWindowRequest AddBookingWindowRequest): IRequest<Result>;

public class AddBookingWindowCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<AddBookingWindowCommand, Result>
{
    public async Task<Result> Handle(AddBookingWindowCommand command, CancellationToken cancellationToken)
    {
        var request = command.AddBookingWindowRequest;
        //Validate du lieu tu request
        request.Validate();
        
        var venue = await unitOfWork.Venue.GetById(request.VenueId);
        if (venue is null) return VenueErrors.VenueNotFound;

        //Tao moi booking window
        var bookingWindow = mapper.Map<BookingWindow>(request);
        await unitOfWork.BookingWindow.Create(bookingWindow);

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