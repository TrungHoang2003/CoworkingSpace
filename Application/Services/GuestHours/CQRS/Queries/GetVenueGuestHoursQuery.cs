using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.GuestHours.CQRS.Queries;

public sealed record GetVenueGuestHoursQuery(int VenueId): IRequest<Result<List<GuestHour>>>;

public class GetVenueGuestHoursQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetVenueGuestHoursQuery, Result<List<GuestHour>>>
{
    public async Task<Result<List<GuestHour>>> Handle(GetVenueGuestHoursQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Venue.FindById(request.VenueId);
        if (!result) return Result<List<GuestHour>>.Failure(VenueErrors.VenueNotFound);
        
        var guestHours = await unitOfWork.GuestHour.GetGuestHoursByVenueId(request.VenueId);
        return Result<List<GuestHour>>.Success(guestHours);
    }
}