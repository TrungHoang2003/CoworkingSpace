using Domain.Entities;
using Domain.ResultPattern;
using MediatR;

namespace Application.SpaceService.CQRS.Commands;

public sealed record SetUpDailySpaceCommand(
    int VenueId,
    int SpaceTypeId
) : IRequest<Result>;

public class SetUpDailySpaceCommandHandler : IRequestHandler<SetUpDailySpaceCommand, Result>
{
    public async Task<Result> Handle(SetUpDailySpaceCommand request, CancellationToken cancellationToken)
    {
        
    }
}