using Application.VenueService.CQRS.Commands;
using Domain.Entities;
using Domain.ViewModel;

namespace Application.Services.Venues.Mappings;

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
}