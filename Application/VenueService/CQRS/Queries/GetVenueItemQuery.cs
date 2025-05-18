using Domain.Errors;
using Domain.ResultPattern;
using Domain.ViewModel;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.VenueService.CQRS.Queries;

public sealed record GetVenueItemQuery(int VenueId):IRequest<Result<VenueItemViewModel>>;

public class GetVenueItemQueryHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) : IRequestHandler<GetVenueItemQuery, Result<VenueItemViewModel>>
{
    public async Task<Result<VenueItemViewModel>> Handle(GetVenueItemQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        int.TryParse(userId, out var hostId);
        var venue = await unitOfWork.Venue.GetVenueItem(hostId, request.VenueId);
        if (venue == null) return Result<VenueItemViewModel>.Failure(VenueErrors.VenueNotFound);
        return Result<VenueItemViewModel>.Success(venue);
    }
}