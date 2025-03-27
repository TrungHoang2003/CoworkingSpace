namespace Infrastructure.Interfaces;

public interface IUnitOfWork
{
   IVenueRepository Venue { get; }
   IUserRepository User { get; }
   Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}