using Application.VenueService.CQRS.Commands;
using Domain.Entities;

namespace Application.VenueService.Mappings;

public static class VenueMapping
{
    // Ánh xạ từ SignUpVenueCommand sang Venue
    public static Venue ToVenue(this SignUpVenueCommand command, string? logoUrl = null)
    {
        return new Venue
        {
            VenueTypeId = command.VenueTypeId,
            Name = command.Name,
            Description = command.Description,
            Floor = command.Floor,
            LogoUrl = logoUrl, // Được truyền vào từ kết quả upload Cloudinary
            Address = new VenueAddress
            {
                Street = command.Address.Street,
                District = command.Address.District,
                City = command.Address.City,
                Latitude = command.Address.Latitude,
                Longitude = command.Address.Longitude
            }
        };
    }

    public static Venue ToVenue(this SetUpVenueCommand command, Venue existingVenue)
    {
        existingVenue.Name = command.Details?.Name;
        existingVenue.Description = command.Details?.Description;
        existingVenue.Floor = command.Details?.Floor;
        existingVenue.Address.Street = command.Address?.Street;
        existingVenue.Address.District = command.Address?.District;
        existingVenue.Address.City = command.Address?.City;
        existingVenue.Address.Latitude = command.Address?.Latitude;
        existingVenue.Address.Longitude = command.Address?.Longitude;
        return existingVenue;
    }
}