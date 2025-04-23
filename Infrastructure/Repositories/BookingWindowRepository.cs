using Domain.Entities;
using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface IBookingWindowRepository : IGenericRepository<BookingWindow>
{
    // Add any additional methods specific to BookingWindow here
}
public class BookingWindowRepository(ApplicationDbContext dbContext) : GenericRepository<BookingWindow>(dbContext), IBookingWindowRepository
{
    
}