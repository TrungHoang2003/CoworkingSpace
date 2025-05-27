using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.GuestHours.CQRS.Queries;

public sealed record GetVenueGuestHoursQuery(int VenueId): IRequest<Result<GuestHour>>;

public class GetVenueGuestHoursQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetVenueGuestHoursQuery, Result<GuestHour>>
{
    public async Task<Result<GuestHour>> Handle(GetVenueGuestHoursQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Venue.FindById(request.VenueId);
        if (!result) return Result<GuestHour>.Failure(VenueErrors.VenueNotFound);
        
        var guestHours = await unitOfWork.GuestHour.GetGuestHoursByVenueId(request.VenueId);
        if (guestHours == null) return Result<GuestHour>.Failure(GuestHourErrors.GuestHourNotFound);
        return Result<GuestHour>.Success(guestHours);
    }
}