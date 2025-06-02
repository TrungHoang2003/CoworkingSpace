using Dapper;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.DbHelper;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Infrastructure.Repositories;

public interface ISpaceRepository : IGenericRepository<Space>
{
    Task<Space?> GetById(int spaceId);
    Task<bool> CheckAvailability(int spaceId, DateTime startDate, DateTime endDate);
    Task<List<Space>> GetAllWorkingSpacesAsync();
    Task<List<Space>> GetVenueWorkingSpacesAsync(int venueId);
    Task<bool> FindById(int spaceId);
    Task<Space?> GetByIdAndVenue(int spaceId, int venueId);
    Task<List<Space>> GetSpaces(SpaceFilter filter);
    Task<List<Space>> SearchSpaces(string searchTerm);
    Task<List<string>> GetSearchSuggestions(string searchTerm);
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
        .Join("Price as p", "s.PriceId", "p.Id")
        .Select("s.*")
        .Where("p.Amount", ">=", filter.MinPrice)
        .Where("p.Amount", "<=", filter.MaxPrice);

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

        if (filter.VenueTypeId.HasValue)
        {
            query.Where("s.VenueTypeId", filter.VenueTypeId);
        }

        if (filter.AmenityIds is { Count: > 0 })
        {
            query.WhereIn("s.SpaceId", db.Query("SpaceAmenity").Select("SpaceId").WhereIn("AmenityId", filter.AmenityIds));
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
    public async Task<List<Space>> SearchSpaces(string searchTerm)
    {
        var cnn = dbConnection.OpenConnection();

        var db = new QueryFactory(cnn, compiler);
        var query = db.Query("Space as s")
            .Join("Venue as v", "s.VenueId", "v.VenueId")
            .Join("VenueAddress as va", "v.VenueAddressId", "va.VenueAddressId")
            .Select("s.*")
            .WhereLike("s.Name", $"%{searchTerm}%")
            .OrWhereLike("va.FullAddress", $"%{searchTerm}%");
        var result = await query.GetAsync<Space>();
        return result.ToList();
    }

    public async Task<List<string>> GetSearchSuggestions(string keyWord)
    {
        if (string.IsNullOrWhiteSpace(keyWord) || keyWord.Length < 2)
        {
            return new List<string>();
        }

        await using var cnn = dbConnection.OpenConnection();
        var db = new QueryFactory(cnn, compiler);

        var suggestions = new List<string>();

        try
        {
            // 1. Tên Space
            var spaceNames = await db.Query("Space")
                .Select("Name")
                .Where("Name", "LIKE", $"%{keyWord}%")
                .WhereNotNull("Name")
                .Distinct()
                .Limit(5)
                .GetAsync<string>();
            suggestions.AddRange(spaceNames.Where(x => !string.IsNullOrEmpty(x)));

            // 2. Tên Venue
            var venueNames = await db.Query("Venue")
                .Select("Name")
                .Where("Name", "LIKE", $"%{keyWord}%")
                .WhereNotNull("Name")
                .Distinct()
                .Limit(3)
                .GetAsync<string>();
            suggestions.AddRange(venueNames.Where(x => !string.IsNullOrEmpty(x)));

            // 3. City từ VenueAddress
            var cities = await db.Query("VenueAddress")
                .Select("City")
                .Where("City", "LIKE", $"%{keyWord}%")
                .WhereNotNull("City")
                .Distinct()
                .Limit(3)
                .GetAsync<string>();
            suggestions.AddRange(cities.Where(x => !string.IsNullOrEmpty(x)));

            // 4. State từ VenueAddress
            var states = await db.Query("VenueAddress")
                .Select("State")
                .Where("State", "LIKE", $"%{keyWord}%")
                .WhereNotNull("State")
                .Distinct()
                .Limit(2)
                .GetAsync<string>();
            suggestions.AddRange(states.Where(x => !string.IsNullOrEmpty(x)));
        }
        catch (Exception)
        {
            // Log error if needed
            return new List<string>();
        }

        // Loại bỏ trùng lặp và giới hạn 10 kết quả
        return suggestions
            .Distinct()
            .Take(10)
            .OrderBy(s => s.Length)
            .ToList();
    }
}