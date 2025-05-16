using Application.VenueAddressService.DTOs;
using Domain.Entities;

namespace Application.VenueAddressService.Mappings;

public static class VenueAddressMapping
{
    public static VenueAddress ToVenueAddress(this SetUpVenueAddressDto dto)
    {
        return new VenueAddress
        {
            District = dto.District,
            Street = dto.Street,
            City = dto.City,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
    }
}