using Domain.Entities;
using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface IGuestArrivalRepository : IGenericRepository<GuestArrival>;

public class GuestArrivalRepository(ApplicationDbContext dbContext) :GenericRepository<GuestArrival>(dbContext), IGuestArrivalRepository
{
    
}