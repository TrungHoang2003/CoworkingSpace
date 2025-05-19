using Domain.Entities;
using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface IVenueTypeRepository : IGenericRepository<VenueType>;
public class VenueTypeRepository(ApplicationDbContext dbContext) : GenericRepository<VenueType>(dbContext), IVenueTypeRepository
{
    
}