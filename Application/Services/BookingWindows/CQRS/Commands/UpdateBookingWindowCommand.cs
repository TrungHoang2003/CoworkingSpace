using Application.BookingWindowService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.BookingWindowService.CQRS.Commands;

public sealed record UpdateBookingWindowCommand(
    int BookingWindowId,
    int VenueId,
    int MinNotice,
    int? MaxNoticeDays,
    BookingTimeUnit Unit,
    bool? DisplayOnCalendar,
    bool ApplyAll,
    List<int>? SpaceIds
) : IRequest<Result>;

public class UpdateBookingWindowCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBookingWindowCommand, Result>
{
    public async Task<Result> Handle(UpdateBookingWindowCommand request, CancellationToken cancellationToken)
    {
        var bookingWindow = await unitOfWork.BookingWindow.GetById(request.BookingWindowId);
        if (bookingWindow is null) return BookingWindowErrors.BookingWindowNotFound;

        //Update booking window
        bookingWindow = request.ToBookingWindow(bookingWindow);
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