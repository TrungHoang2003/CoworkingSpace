using Dapper;
using Domain.Entites;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public interface ISpaceRepository: IGenericRepository<Space>
{
   Task<List<Space>> GetAllWorkingSpacesAsync();
   Task<List<Space>> GetVenueWorkingSpacesAsync(int venueId);
   Task<Space?> GetById(int spaceId);
   Task<Space?> GetByIdAndVenue(int spaceId, int venueId);
} 
public class SpaceRepository(ApplicationDbContext dbContext, IConfiguration configuration) : GenericRepository<Space>(dbContext), ISpaceRepository
{
    public async Task<List<Space>> GetAllWorkingSpacesAsync()
    {
        var connection = new MySqlServer(configuration).OpenConnection();
        var sql = $"select * from Space";
        var result = await connection.QueryAsync<Space>(sql);

        return result.ToList();
    }

    public async Task<List<Space>> GetVenueWorkingSpacesAsync(int venueId)
    {
        var connection = new MySqlServer(configuration).OpenConnection();
        var sql = $"select * from Space where VenueId = @venueId";
        var result = await connection.QueryAsync<Space>(sql, new { VenueId = venueId });

        return result.ToList();
    }

    public async Task<Space?> GetById(int spaceId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();
        var sql = "Select * from Space where SpaceId = @spaceId";
        var result = await cnn.QueryFirstOrDefaultAsync<Space>(sql, new { SpaceId = spaceId });

        return result;
    }

    public async Task<Space?> GetByIdAndVenue(int spaceId, int venueId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();
        const string sql = "Select * from Space where SpaceId = @spaceId and VenueId = @venueId";
        var result = await cnn.QueryFirstOrDefaultAsync<Space>(sql, new { SpaceId = spaceId, VenueId = venueId });
        
        return result;
    }
}