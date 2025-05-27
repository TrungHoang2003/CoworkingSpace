using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface IUnitOfWork
{
    IReviewRepository Review { get; }
   IVenueRepository Venue { get; }
   IPriceRepository Price { get; }
   IReservationRepository Reservation { get; }
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
   IGuestArrivalRepository GuestArrival { get; }
   ISpaceAmenityRepository SpaceAmenity { get; }
   ISpaceCollectionRepository SpaceCollection { get; }
   ICollectionRepository Collection { get; }
   Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork(ApplicationDbContext dbContext, IVenueRepository venue, IUserRepository user, IVenueTypeRepository venueType, IVenueAddressRepository venueAddress, IGuestHourRepository guestHour, IHolidayRepository holiday, IVenueHolidayRepository venueHoliday, IBookingWindowRepository bookingWindow, ISpaceRepository space, IExceptionRepository exception, ISpaceTypeRepository spaceType, IAmenityRepository amenity, ISpaceAmenityRepository spaceAmenity, ISpaceAssetRepository spaceAsset, IGuestArrivalRepository guestArrival, IReservationRepository reservation,
    IPriceRepository price, ICollectionRepository collection,
    ISpaceCollectionRepository spaceCollection,
    IReviewRepository review) : IUnitOfWork
{
    public IReviewRepository Review { get; } = review;
    public IVenueRepository Venue { get; } = venue;
    public IPriceRepository Price { get; } = price;
    public IReservationRepository Reservation { get; } = reservation;
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
    public IGuestArrivalRepository GuestArrival { get; } = guestArrival;
    public ISpaceAmenityRepository SpaceAmenity { get; } = spaceAmenity;
    public ISpaceCollectionRepository SpaceCollection { get; } = spaceCollection;
    public ICollectionRepository Collection { get; } = collection;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
       return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public IHolidayRepository Holiday { get; } = holiday;
}