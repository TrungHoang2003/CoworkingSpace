using MediatR;

namespace Application.VenueService.Commands;

public sealed record UpdateGuestHoursCommand : IRequest;

public class UpdateGuestHoursCommandHandler: IRequestHandler<UpdateGuestHoursCommand>
{
    public Task Handle(UpdateGuestHoursCommand request, CancellationToken cancellationToken)
    {
        // Logic to update guest arrival
        return Task.FromResult(Unit.Value);
    }
}