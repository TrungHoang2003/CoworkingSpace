using Domain.Entites;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IVenueAddressRepository: IGenericRepository<VenueAddress>
{
    void UpdateVenueFullAddress(VenueAddress venueAddress);
    
    Task<VenueAddress?> GetVenueAddressById(int venueAddressId);
}