using Application.BookingWindowService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.BookingWindowService.CQRS.Commands;

public sealed record AddBookingWindowCommand(
    int VenueId,
    int MinNotice,
    int? MaxNoticeDays,
    BookingTimeUnit Unit,
    bool? DisplayOnCalendar,
    bool ApplyAll,
    List<int>? SpaceIds
) : IRequest<Result>;

public class AddBookingWindowCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddBookingWindowCommand, Result>
{
    public async Task<Result> Handle(AddBookingWindowCommand request, CancellationToken cancellationToken)
    {
        var venue = await unitOfWork.Venue.GetById(request.VenueId);
        if (venue is null) return VenueErrors.VenueNotFound;

        //Tao moi booking window
        var bookingWindow = request.ToBookingWindow();
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
        else
        {
            foreach (var spaceId in request.SpaceIds!)
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