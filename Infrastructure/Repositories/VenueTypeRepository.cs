using Domain.Entites;
using Domain.Entities;
using Infrastructure.DbHelper;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class VenueTypeRepository(ApplicationDbContext dbContext) : GenericRepository<VenueType>(dbContext), IVenueTypeRepository
{
    
}