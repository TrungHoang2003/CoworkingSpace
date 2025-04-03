using Domain.DTOs;
using Infrastructure.Common;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.VenueService.Commands;

public sealed record UpdateVenueDetailsCommand(UpdateVenueDetailsDTO UpdateVenueDetailsDto) : IRequest<Result>;

public class UpdateVenueDetailsCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateVenueDetailsCommand, Result>
{
    public async Task<Result> Handle(UpdateVenueDetailsCommand command, CancellationToken cancellationToken)
    {
        var venue = await unitOfWork.Venue.GetVenueById(command.UpdateVenueDetailsDto.VenueId);
        if (venue == null)
            return VenueErrors.VenueNotFound;
        
        var venueAddress = await unitOfWork.VenueAddress.GetAddressByVenueId(command.UpdateVenueDetailsDto.VenueId);
        if(venueAddress == null)
            return VenueErrors.VenueNotFound;

        venue.Name = command.UpdateVenueDetailsDto.VenueName;
        venue.Description = command.UpdateVenueDetailsDto.VenueDescription;
        venue.Address.Street = command.UpdateVenueDetailsDto.VenueStreet;
        venue.Address.City = command.UpdateVenueDetailsDto.VenueCity;
        venue.Address.District = command.UpdateVenueDetailsDto.VenueDistrict;
        venue.Address.Latitude = command.UpdateVenueDetailsDto.VenueLatitude;
        venue.Address.Longitude = command.UpdateVenueDetailsDto.VenueLongitude;
        venue.Floor = command.UpdateVenueDetailsDto.VenueFloor;

        await unitOfWork.Venue.Update(venue);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}