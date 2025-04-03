using Domain.Entites;
using Infrastructure.DbHelper;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class VenueTypeRepository(ApplicationDbContext dbContext) : GenericRepository<VenueType>(dbContext), IVenueTypeRepository
{
    
}