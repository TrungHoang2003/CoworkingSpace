using Domain.Entities;
using Infrastructure.DbHelper;

namespace Infrastructure.Repositories;

public interface ISpaceImageRepository : IGenericRepository<SpaceImage>;
public class SpaceImageRepository(ApplicationDbContext dbContext) :GenericRepository<SpaceImage>(dbContext), ISpaceImageRepository
{
    
}