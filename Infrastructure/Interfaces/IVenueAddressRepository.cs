using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IVenueAddressRepository: IGenericRepository<VenueAddress>
{
    void UpdateVenueFullAddress(VenueAddress venueAddress);
    Task<VenueAddress?> GetAddressByVenueId(int venueId);
}