using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface ISpaceAmenityRepository : IGenericRepository<SpaceAmenity>
{
    Task<SpaceAmenity?> Get(int amenityId, int spaceId);
    void RemoveRange(List<SpaceAmenity> spaceAmenities);
    Task AddRange(List<SpaceAmenity> spaceAmenities);
}

public class SpaceAmenityRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) : GenericRepository<SpaceAmenity>(dbContext), ISpaceAmenityRepository
{
    public async  Task<SpaceAmenity?> Get(int amenityId, int spaceId)
    {
        var cnn = dbConnection.OpenConnection();
        
        var sql = $"select * from SpaceAmenity where AmenityId = {amenityId} and SpaceId = {spaceId}";
        var result = await cnn.QueryFirstOrDefaultAsync(sql, new {amenityId, spaceId});
        return result;
    }

    public void RemoveRange(List<SpaceAmenity> spaceAmenities)
    {
        dbContext.RemoveRange(spaceAmenities);
    }

    public async Task AddRange(List<SpaceAmenity> spaceAmenities)
    {
        await dbContext.AddRangeAsync(spaceAmenities);
    }
}