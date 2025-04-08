using Domain.Entities;
using Infrastructure.DbHelper;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class BookingWindowRepository(ApplicationDbContext dbContext) : GenericRepository<BookingWindow>(dbContext), IBookingWindowRepository
{
    
}