using Dapper;
using Domain.Entites;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IVenueRepository: IGenericRepository<Venue>
{
   Task<IEnumerable<VenueType>> GetVenueTypes();
   Task<Venue?> GetVenuesByTypeId(int venueTypeId);
   Task<VenueType?> GetVenueTypeById(int venueTypeId);
   Task<Venue?> GetById(int venueId);
   Task<bool> FindById(int venueId);
   Task<List<Venue>> GetVenuesByHostId(int hostId);
}

public class VenueRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) : GenericRepository<Venue>(dbContext), IVenueRepository
{
    public async Task<IEnumerable<VenueType>> GetVenueTypes()
    {
        var cnn = dbConnection.OpenConnection();
        try
        {
            const string sql = "select * from VenueType";
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
        var cnn = dbConnection.OpenConnection();
        try
        {
            const string sql = "select * from Venue where VenueTypeId = @venueTypeId";
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
        var cnn = dbConnection.OpenConnection();

        try
        {
            const string sql = "select * from VenueType where VenueTypeId = @venueTypeId";
            var result = await cnn.QueryFirstOrDefaultAsync<VenueType>(sql, new { VenueTypeId = venueTypeId });
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting venueType by id", e);
        }
    }

    public async Task<Venue?> GetById(int venueId)
    {
        var cnn = dbConnection.OpenConnection();

        try
        {
            const string sql = "select * from Venue where VenueId = @venueId";
            var result = await cnn.QueryFirstOrDefaultAsync<Venue>(sql, new { VenueId = venueId });
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting venues by type id", e);
        }
    }

    public async Task<bool> FindById(int venueId)
    {
        var cnn = dbConnection.OpenConnection();
        
        const string sql = "select count(*) from Venue where VenueId = @venueId";
        var result = await cnn.ExecuteScalarAsync<int>(sql, new { VenueId = venueId });
        return result > 0;
    }

    public async Task<List<Venue>> GetVenuesByHostId(int hostId)
    {
        var cnn = dbConnection.OpenConnection();
        try
        {
            const string sql = "select * from Venue where HostId = @hostId";
            var result = await cnn.QueryAsync<Venue>(sql, new { HostId= hostId });
            return result.ToList();
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting venues by user id", e);
        }
    }
}