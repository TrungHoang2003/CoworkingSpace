using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public interface ISpaceAmenityRepository : IGenericRepository<SpaceAmenity>
{
    Task<SpaceAmenity?> Get(int amenityId, int spaceId);
}

public class SpaceAmenityRepository(ApplicationDbContext dbContext, IConfiguration configuration) : GenericRepository<SpaceAmenity>(dbContext), ISpaceAmenityRepository
{
    public async  Task<SpaceAmenity?> Get(int amenityId, int spaceId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();
        
        var sql = $"select * from SpaceAmenity where AmenityId = {amenityId} and SpaceId = {spaceId}";
        var result = await cnn.QueryFirstOrDefaultAsync(sql, new {amenityId, spaceId});
        return result;
    }
}