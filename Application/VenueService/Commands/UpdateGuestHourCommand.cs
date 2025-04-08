using MediatR;

namespace Application.VenueService.Commands;

public sealed record UpdateGuestHourCommand : IRequest;

public class UpdateGuestHourCommandHandler: IRequestHandler<UpdateGuestHourCommand>
{
    public Task Handle(UpdateGuestHourCommand request, CancellationToken cancellationToken)
    {
        // Logic to update guest arrival
        return Task.FromResult(Unit.Value);
    }
}