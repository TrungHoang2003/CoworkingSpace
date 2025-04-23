using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface IUnitOfWork
{
   IVenueRepository Venue { get; }
   IGuestHourRepository GuestHour { get; }
   IVenueTypeRepository VenueType { get; }
   IVenueAddressRepository VenueAddress{ get; }
   IUserRepository User { get; }
   IVenueHolidayRepository VenueHoliday { get; }
   IHolidayRepository Holiday { get; }
   Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork(ApplicationDbContext dbContext, IVenueRepository venue, IUserRepository user, IVenueTypeRepository venueType, IVenueAddressRepository venueAddress, IGuestHourRepository guestHour, IHolidayRepository holiday, IVenueHolidayRepository venueHoliday) : IUnitOfWork
{
    public IVenueRepository Venue { get; } = venue;
    public IGuestHourRepository GuestHour { get; } = guestHour;
    public IVenueTypeRepository VenueType { get; } = venueType;
    public IUserRepository User { get; } = user;
    public IVenueHolidayRepository VenueHoliday { get; } = venueHoliday;
    public IVenueAddressRepository VenueAddress { get; } = venueAddress;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
       return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public IHolidayRepository Holiday { get; } = holiday;
}