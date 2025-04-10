using Domain.Entites;
using Domain.Entities;
using Infrastructure.Common;

namespace Infrastructure.Interfaces;

public interface IVenueRepository: IGenericRepository<Venue>
{
   Task<IEnumerable<VenueType>> GetVenueTypes();
   Task<Venue?> GetVenuesByTypeId(int venueTypeId);
   Task<VenueType?> GetVenueTypeById(int venueTypeId);
   Task<Venue?> GetVenueById(int venueId);
}