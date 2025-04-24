using Domain.Entities;
using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface IBookingWindowRepository : IGenericRepository<BookingWindow>
{
    
}
public class BookingWindowRepository(ApplicationDbContext dbContext) : GenericRepository<BookingWindow>(dbContext), IBookingWindowRepository
{
    
}