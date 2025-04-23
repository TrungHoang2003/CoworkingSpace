using Domain.DTOs;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Errors;
using Infrastructure.Repositories;
using MediatR;

namespace Application.VenueService.Commands;

public sealed record UpdateVenueDetailsCommand(UpdateVenueDetailsRequest UpdateVenueDetailsRequest) : IRequest<Result>;

public class UpdateVenueDetailsCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateVenueDetailsCommand, Result>
{
    public async Task<Result> Handle(UpdateVenueDetailsCommand command, CancellationToken cancellationToken)
    {
        var venue = await unitOfWork.Venue.GetById(command.UpdateVenueDetailsRequest.VenueId);
        if (venue == null) return VenueErrors.VenueNotFound;
        
        var venueAddress = await unitOfWork.VenueAddress.GetVenueAddressById(venue.VenueAddressId);
        if(venueAddress == null) return VenueErrors.VenueNotFound;

        venue.Name = command.UpdateVenueDetailsRequest.VenueName;
        venue.Description = command.UpdateVenueDetailsRequest.VenueDescription;
        venueAddress.Street = command.UpdateVenueDetailsRequest.VenueStreet;
        venueAddress.City = command.UpdateVenueDetailsRequest.VenueCity;
        venueAddress.District = command.UpdateVenueDetailsRequest.VenueDistrict;
        venueAddress.Latitude = command.UpdateVenueDetailsRequest.VenueLatitude;
        venueAddress.Longitude = command.UpdateVenueDetailsRequest.VenueLongitude;
        venue.Floor = command.UpdateVenueDetailsRequest.VenueFloor;

        await unitOfWork.Venue.Update(venue);
        await unitOfWork.VenueAddress.Update(venueAddress);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}