using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.VenueService.CQRS.Queries;

public sealed record GetUserVenuesQuery:IRequest<Result<List<Venue>>>;

public class GetUserVenuesHandler(IHttpContextAccessor httpContextAccessor,IUnitOfWork unitOfWork) : IRequestHandler<GetUserVenuesQuery, Result<List<Venue>>>
{
    public async Task<Result<List<Venue>>> Handle(GetUserVenuesQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        
        int.TryParse(userId, out var hostId); 
        var venues = await unitOfWork.Venue.GetVenuesByHostId(hostId);

        return Result<List<Venue>>.Success(venues);
    }
}