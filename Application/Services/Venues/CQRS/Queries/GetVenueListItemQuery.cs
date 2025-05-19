using Domain.ResultPattern;
using Domain.ViewModel;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.VenueService.CQRS.Queries;

public sealed record GetVenueListItemQuery:IRequest<Result<List<VenueItemViewModel>>>;

public class GetVenueListItemQueryHandler(IHttpContextAccessor httpContextAccessor,IUnitOfWork unitOfWork) : IRequestHandler<GetVenueListItemQuery, Result<List<VenueItemViewModel>>>
{
    public async Task<Result<List<VenueItemViewModel>>> Handle(GetVenueListItemQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        int.TryParse(userId, out var hostId);
        var venues = await unitOfWork.Venue.GetVenueListItem(hostId);
        return Result<List<VenueItemViewModel>>.Success(venues);
    }
}