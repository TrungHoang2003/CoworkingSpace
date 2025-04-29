using Domain.Entities;
using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface ISpaceAmenityRepository : IGenericRepository<SpaceAmenity>;

public class SpaceAmenityRepository(ApplicationDbContext dbContext) : GenericRepository<SpaceAmenity>(dbContext), ISpaceAmenityRepository;