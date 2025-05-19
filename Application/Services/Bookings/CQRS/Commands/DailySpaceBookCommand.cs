using MediatR;

namespace Application.Services.Bookings.CQRS.Commands;

public sealed record DailySpaceBookCommand(
    int SpaceId,
    DateTime StartDate,
    DateTime EndDate,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int? Quantity
    ):IRequest;