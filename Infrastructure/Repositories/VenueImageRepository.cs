using Domain.Entities;
using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface IVenueImageRepository : IGenericRepository<VenueImage>;
public class VenueImageRepository(ApplicationDbContext dbContext) : GenericRepository<VenueImage>(dbContext), IVenueImageRepository
{
}