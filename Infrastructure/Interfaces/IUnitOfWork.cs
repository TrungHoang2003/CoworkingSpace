namespace Infrastructure.Interfaces;

public interface IUnitOfWork
{
   IVenueRepository Venue { get; }
   IVenueTypeRepository VenueType { get; }
   IVenueAddressRepository VenueAddress{ get; }
   IUserRepository User { get; }
   Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}