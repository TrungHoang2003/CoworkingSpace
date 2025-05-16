using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface IUnitOfWork
{
   IVenueRepository Venue { get; }
    ISpaceTypeRepository SpaceType { get; }    
   ISpaceRepository Space { get; }
   IGuestHourRepository GuestHour { get; }
   ISpaceAssetRepository SpaceAsset { get; }
   IVenueTypeRepository VenueType { get; }
   IVenueAddressRepository VenueAddress{ get; }
   IUserRepository User { get; }
   IVenueHolidayRepository VenueHoliday { get; }
   IHolidayRepository Holiday { get; }
   IBookingWindowRepository BookingWindow { get; }
   IExceptionRepository Exception { get; }
   IAmenityRepository Amenity { get; }
   ISpaceAmenityRepository SpaceAmenity { get; }
   Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork(ApplicationDbContext dbContext, IVenueRepository venue, IUserRepository user, IVenueTypeRepository venueType, IVenueAddressRepository venueAddress, IGuestHourRepository guestHour, IHolidayRepository holiday, IVenueHolidayRepository venueHoliday, IBookingWindowRepository bookingWindow, ISpaceRepository space, IExceptionRepository exception, ISpaceTypeRepository spaceType, IAmenityRepository amenity, ISpaceAmenityRepository spaceAmenity, ISpaceAssetRepository spaceAsset) : IUnitOfWork
{
    public IVenueRepository Venue { get; } = venue;
    public ISpaceTypeRepository SpaceType { get; } = spaceType;
    public ISpaceRepository Space { get; } = space;
    public IGuestHourRepository GuestHour { get; } = guestHour;
    public ISpaceAssetRepository SpaceAsset { get; } = spaceAsset;
    public IVenueTypeRepository VenueType { get; } = venueType;
    public IUserRepository User { get; } = user;
    public IVenueHolidayRepository VenueHoliday { get; } = venueHoliday;
    public IVenueAddressRepository VenueAddress { get; } = venueAddress;

    public IBookingWindowRepository BookingWindow { get; } = bookingWindow;
    public IExceptionRepository Exception { get; } = exception;
    public IAmenityRepository Amenity { get; } = amenity;
    public ISpaceAmenityRepository SpaceAmenity { get; } = spaceAmenity;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
       return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public IHolidayRepository Holiday { get; } = holiday;
}