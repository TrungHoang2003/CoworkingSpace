using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface ISpaceRepository: IGenericRepository<Space>
{
   Task<List<Space>> GetAllWorkingSpacesAsync();
   Task<List<Space>> GetVenueWorkingSpacesAsync(int venueId);
   Task<Space?> GetById(int spaceId);
   Task<bool> FindById(int spaceId);
   Task<Space?> GetByIdAndVenue(int spaceId, int venueId);
} 
public class SpaceRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) : GenericRepository<Space>(dbContext), ISpaceRepository
{
    public async Task<List<Space>> GetAllWorkingSpacesAsync()
    {
        var connection = dbConnection.OpenConnection();
        var sql = $"select * from Space";
        var result = await connection.QueryAsync<Space>(sql);

        return result.ToList();
    }

    public async Task<List<Space>> GetVenueWorkingSpacesAsync(int venueId)
    {
        var connection =dbConnection.OpenConnection();
        var sql = $"select * from Space where VenueId = @venueId";
        var result = await connection.QueryAsync<Space>(sql, new { VenueId = venueId });

        return result.ToList();
    }

    public async Task<Space?> GetById(int spaceId)
    {
        var cnn = dbConnection.OpenConnection();
        var sql = "Select * from Space where SpaceId = @spaceId";
        var result = await cnn.QueryFirstOrDefaultAsync<Space>(sql, new { SpaceId = spaceId });

        return result;
    }

    public async Task<bool> FindById(int spaceId)
    {
        var cnn = dbConnection.OpenConnection();
        const string sql = "Select COUNT(*) from Space where SpaceId = @spaceId";
        var result = await cnn.ExecuteScalarAsync<int>(sql, new { SpaceId = spaceId });
        return result > 0;
    }

    public async Task<Space?> GetByIdAndVenue(int spaceId, int venueId)
    {
        var cnn = dbConnection.OpenConnection();
        const string sql = "Select * from Space where SpaceId = @spaceId and VenueId = @venueId";
        var result = await cnn.QueryFirstOrDefaultAsync<Space>(sql, new { SpaceId = spaceId, VenueId = venueId });
        
        return result;
    }
}