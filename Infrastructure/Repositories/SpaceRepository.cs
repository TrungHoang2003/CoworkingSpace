using System.Data.SqlTypes;
using System.Text;
using Dapper;
using Domain.Entities;
using Domain.Filters;
using Google.Apis.Requests;
using Infrastructure.DbHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using MySqlConnector;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Infrastructure.Repositories;

public interface ISpaceRepository : IGenericRepository<Space>
{
    Task<bool> CheckAvailability(int spaceId, DateTime startDate, DateTime endDate);
    Task<List<Space>> GetAllWorkingSpacesAsync();
    Task<List<Space>> GetVenueWorkingSpacesAsync(int venueId);
    Task<Space?> GetById(int spaceId);
    Task<bool> FindById(int spaceId);
    Task<Space?> GetByIdAndVenue(int spaceId, int venueId);
    Task<List<Space>> GetSpaces(SpaceFilter filter);
}

public class SpaceRepository(MySqlCompiler compiler, ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection)
    : GenericRepository<Space>(dbContext), ISpaceRepository
{
    public async Task<List<Space>> GetAllWorkingSpacesAsync()
    {
        var connection = dbConnection.OpenConnection();
        const string sql = $"select * from Space";
        var result = await connection.QueryAsync<Space>(sql);

        return result.ToList();
    }

    public async Task<List<Space>> GetVenueWorkingSpacesAsync(int venueId)
    {
        var connection = dbConnection.OpenConnection();
        const string sql = $"select * from Space where VenueId = @venueId";
        var result = await connection.QueryAsync<Space>(sql, new { VenueId = venueId });

        return result.ToList();
    }

    public async Task<Space?> GetById(int spaceId)
    {
        var cnn = dbConnection.OpenConnection();
        const string sql = "Select * from Space where SpaceId = @spaceId";
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
        await using var cnn = dbConnection.OpenConnection();
        const string sql = "Select * from Space where SpaceId = @spaceId and VenueId = @venueId";
        var result = await cnn.QueryFirstOrDefaultAsync<Space>(sql, new { SpaceId = spaceId, VenueId = venueId });

        return result;
    }

    public async Task<List<Space>> GetSpaces(SpaceFilter filter)
    {
        await using var cnn = dbConnection.OpenConnection();
        var db = new QueryFactory(cnn, compiler);

        var query = db.Query("Space as s")
            .Select("s.*")
            .Where(q => q
                .From("Price as p")
                .WhereColumns("s.PriceId", "=", "p.Id")
                .Where("p.Amount", ">=", filter.MinPrice)
                .Where("p.Amount", "<=", filter.MaxPrice));

        if (filter is { StartDate: not null, EndDate: not null })
        {
            query.WhereNotExists(q => q
            .From("Reservation as r")
            .WhereColumns("r.SpaceId", "=", "s.SpaceId")
            .Where("r.StartDate", "<", filter.EndDate)
            .Where("r.EndDate", ">", filter.StartDate)
        );
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query.Where("s.Name", "LIKE", $"%{filter.Name}%");
        }

        if (filter.ListingType.HasValue)
        {
            query.Where("s.ListingType", filter.ListingType.Value);
        }

        if (filter.Capacity.HasValue)
        {
            query.Where("s.Capacity", filter.Capacity.Value);
        }

        if (filter.SpaceTypeId.HasValue)
        {
            query.Where("s.SpaceTypeId", filter.SpaceTypeId.Value);
        }

        var result = await query.GetAsync<Space>();
        return result.ToList();
    }

    public async Task<bool> CheckAvailability(int spaceId, DateTime startDate, DateTime endDate)
    {
        var cnn = dbConnection.OpenConnection();
        const string sql = """
                           SELECT COUNT(1) 
                           FROM Reservation
                           WHERE SpaceId = @spaceId
                           and StartDate < @endDate
                           and EndDate > @startDate
                           """;
        var result = await cnn.ExecuteScalarAsync<int>(sql, new { SpaceId = spaceId, StartDate = startDate, EndDate = endDate });
        return result > 0;
    }
}