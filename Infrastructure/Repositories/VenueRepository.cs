using Dapper;
using Domain.Entities;
using Domain.ViewModel;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IVenueRepository: IGenericRepository<Venue>
{
    Task<List<DayOfWeek>> GetClosedDays(int venueId);
    Task<VenueDetailsViewModel?> GetVenueDetails(int venueId);
    Task<IEnumerable<VenueType>> GetVenueTypes();
   Task<Venue?> GetVenuesByTypeId(int venueTypeId);
   Task<VenueType?> GetVenueTypeById(int venueTypeId);
   Task<Venue?> GetById(int venueId);
   Task<bool> FindById(int venueId);
   Task<List<VenueItemViewModel>> GetVenueListItem(int hostId);
   Task<VenueItemViewModel?> GetVenueItem(int hostId, int venueId);
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

    public async Task<List<VenueItemViewModel>> GetVenueListItem(int hostId)
    {
        var cnn = dbConnection.OpenConnection();
        try
        {
            const string sql = @"SELECT v.VenueId, v.Name, v.LogoUrl, a.FullAddress
            FROM Venue v
            LEFT JOIN VenueAddress a ON v.VenueAddressId = a.VenueAddressId
            WHERE v.HostId = @HostId";
            
            var result = await cnn.QueryAsync<VenueItemViewModel>(sql, new { HostId= hostId });
            return result.ToList();
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting venue lis item", e);
        }
    }

    public async Task<VenueItemViewModel?> GetVenueItem(int hostId, int venueId)
    {
        var cnn = dbConnection.OpenConnection();
        try
        {
            const string sql = """
                               SELECT v.VenueId, v.Name, v.LogoUrl, a.FullAddress
                                           FROM Venue v
                                           LEFT JOIN VenueAddress a ON v.VenueAddressId = a.VenueAddressId
                                           WHERE v.HostId = @HostId and v.VenueId = @VenueId
                               """;
            
            var result = await cnn.QueryFirstOrDefaultAsync<VenueItemViewModel>(sql, new { VenueId = venueId, HostId= hostId });
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting Venue item", e);
        }
    }
    
    public async Task<VenueDetailsViewModel?> GetVenueDetails(int venueId)
    {
        var cnn = dbConnection.OpenConnection();
        try
        {
            const string sql = """
                               SELECT v.Name, v.Description, v.LogoUrl, a.City, a.District, a.Street
                                           FROM Venue v
                                           LEFT JOIN VenueAddress a ON v.VenueAddressId = a.VenueAddressId
                                           WHERE v.VenueId = @VenueId
                               """;
            
            var result = await cnn.QueryFirstOrDefaultAsync<VenueDetailsViewModel>(sql, new { VenueId= venueId });
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error while getting venue details", e);
        }
    }

    public async Task<List<DayOfWeek>> GetClosedDays(int venueId)
    {
       var cnn = dbConnection.OpenConnection();
         try
         {
             const string sql = "SELECT DayOfWeek FROM GuestHour WHERE VenueId = @VenueId and IsClosed = 1";
             var result = await cnn.QueryAsync<DayOfWeek>(sql, new { VenueId= venueId });
             return result.ToList();
         }
         catch (Exception e)
         {
             throw new Exception("Error while getting closed days", e);
         }
    }
}