using Infrastructure.DbHelper;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext, IVenueRepository venue, IUserRepository user, IVenueTypeRepository venueType, IVenueAddressRepository venueAddress, IGuestHourRepository guestHour) : IUnitOfWork
{
    public IVenueRepository Venue { get; } = venue;
    public IGuestHourRepository GuestHour { get; } = guestHour;
    public IVenueTypeRepository VenueType { get; } = venueType;
    public IUserRepository User { get; } = user;
    public IVenueAddressRepository VenueAddress { get; } = venueAddress;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
       return await dbContext.SaveChangesAsync(cancellationToken);
    }
}