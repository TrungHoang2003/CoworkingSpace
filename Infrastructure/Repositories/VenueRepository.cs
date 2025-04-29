using Dapper;
using Domain.Entites;
using Domain.Entities;
using Infrastructure.Common;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public interface IVenueRepository: IGenericRepository<Venue>
{
   Task<IEnumerable<VenueType>> GetVenueTypes();
   Task<Venue?> GetVenuesByTypeId(int venueTypeId);
   Task<VenueType?> GetVenueTypeById(int venueTypeId);
   Task<Venue?> GetById(int venueId);
   Task<bool> FindById(int venueId);
}

public class VenueRepository(ApplicationDbContext dbContext, IConfiguration configuration) : GenericRepository<Venue>(dbContext), IVenueRepository
{
    public async Task<IEnumerable<VenueType>> GetVenueTypes()
    {
        var cnn = new MySqlServer(configuration).OpenConnection();

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
        var cnn = new MySqlServer(configuration).OpenConnection();

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
        var cnn = new MySqlServer(configuration).OpenConnection();

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
        var cnn = new MySqlServer(configuration).OpenConnection();

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
        var cnn = new MySqlServer(configuration).OpenConnection();
        
        const string sql = "select count(*) from Venue where VenueId = @venueId";
        var result = await cnn.ExecuteScalarAsync<int>(sql, new { VenueId = venueId });
        return result > 0;
    }
}