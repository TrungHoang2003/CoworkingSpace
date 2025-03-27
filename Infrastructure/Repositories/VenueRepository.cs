using Dapper;
using Domain.Entites;
using Infrastructure.Common;
using Infrastructure.DbHelper;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class VenueRepository(ApplicationDbContext dbContext, IConfiguration configuration) : GenericRepository<Venue>(dbContext), IVenueRepository
{
    public async Task<IEnumerable<VenueType>> GetVenueTypes()
    {
        var cnn = new MySqlServer(configuration).OpenConnection();

        try
        {
            const string sql = "select * from VenueTypes";
            var result = await cnn.QueryAsync<VenueType>(sql);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting venue types", e);
        }
    }

    public async Task<Venue?> GetVenuesByTypeId(int venueTypeId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();

        try
        {
            const string sql = "select * from Venues where VenueTypeId = @VenueTypeId";
            var result = await cnn.QueryFirstOrDefaultAsync<Venue>(sql, new { VenueTypeId = venueTypeId });
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting venues by type id", e);
        }
    }

    public async Task<VenueType?> GetVenueTypeById(int venueTypeId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();

        try
        {
            const string sql = "select * from VenueTypes where VenueTypeId = @VenueTypeId";
            var result = await cnn.QueryFirstOrDefaultAsync<VenueType>(sql, new { VenueTypeId = venueTypeId });
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting venues by type id", e);
        }
    }
}