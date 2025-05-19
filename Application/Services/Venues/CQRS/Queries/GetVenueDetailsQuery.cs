using Application.Services.Venues.Mappings;
using Domain.Errors;
using Domain.ResultPattern;
using Domain.ViewModel;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Venues.CQRS.Queries;

public sealed record GetVenueDetailsQuery(int VenueId):IRequest<Result<VenueDetailsViewModel>>;

public class GetVenueDetailsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetVenueDetailsQuery, Result<VenueDetailsViewModel>>
{
    public async Task<Result<VenueDetailsViewModel>> Handle(GetVenueDetailsQuery request, CancellationToken cancellationToken)
    {
        var venueDetails = await unitOfWork.Venue.GetVenueDetails(request.VenueId);
        if (venueDetails == null)
            return Result<VenueDetailsViewModel>.Failure(VenueErrors.VenueNotFound);
        return Result<VenueDetailsViewModel>.Success(venueDetails);
    }
}