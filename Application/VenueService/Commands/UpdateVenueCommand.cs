using Application.VenueService.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using MediatR;

namespace Application.VenueService.Commands;

public sealed record UpdateVenueCommand(UpdateVenueRequest UpdateVenueRequest) : IRequest<Result>;

public class UpdateVenueDetailsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateVenueCommand, Result>
{
    public async Task<Result> Handle(UpdateVenueCommand command, CancellationToken cancellationToken)
    {
        var venue = await unitOfWork.Venue.GetById(command.UpdateVenueRequest.VenueId);
        if (venue == null) return VenueErrors.VenueNotFound;

        mapper.Map(command.UpdateVenueRequest, venue);

        await unitOfWork.Venue.Update(venue);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}