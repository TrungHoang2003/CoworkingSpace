using Dapper;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.DbHelper;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using Domain;

namespace Infrastructure.Repositories;

public interface ISpaceRepository : IGenericRepository<Space>
{
    Task<Space?> GetById(int spaceId);
    Task<bool> CheckAvailability(int spaceId, DateTime startDate, DateTime endDate);
    Task<List<Space>> GetAllWorkingSpacesAsync();
    Task<List<Space>> GetVenueWorkingSpacesAsync(int venueId);
    Task<bool> FindById(int spaceId);
    Task<Space?> GetByIdAndVenue(int spaceId, int venueId);
    Task<List<SpaceViewHolder>> GetSpaces(SpaceFilter filter, int userId);
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
    public async Task<List<SpaceViewHolder>> GetSpaces(SpaceFilter filter, int userId)
    {
        await using var cnn = dbConnection.OpenConnection();
        var db = new QueryFactory(cnn, compiler);

        var query = db.Query("Space as s")
            .Join("Price as p", "s.PriceId", "p.Id")
            .Join("Venue as v", "s.VenueId", "v.VenueId")
            .Join("VenueAddress as va", "v.VenueAddressId", "va.VenueAddressId")
            .LeftJoin("SpaceAsset as sa", j => j.On("s.SpaceId", "sa.SpaceId"))
            .LeftJoin("Review as r", "s.SpaceId", "r.SpaceId")
            .LeftJoin("SpaceCollection as sc", "s.SpaceId", "sc.SpaceId")
            .LeftJoin("Collection as c", "sc.CollectionId", "c.CollectionId")
            .Where("c.UserId", userId)
            .Select(
        "s.SpaceId",
        "s.Name",
        "va.FullAddress",
        "p.Amount as Price"
            )
            .SelectRaw("IF(COUNT(sc.SpaceCollectionId) > 0, true, false) as IsLiked")
            .SelectRaw("(SELECT Url FROM SpaceAsset WHERE SpaceId = s.SpaceId ORDER BY Id LIMIT 1) as ImageUrl")
            .SelectRaw("Round(COALESCE(AVG(r.Rating), 0),1) as Rate")
            .Where("p.Amount", ">=", filter.MinPrice)
            .Where("p.Amount", "<=", filter.MaxPrice)
            .GroupBy("s.SpaceId", "s.Name", "va.FullAddress", "p.Amount");

        // Apply conditional filters
        if (filter.Type.HasValue)
        {
            query.Where("s.ListingType", "=", filter.Type.Value);
        }

        if (filter.Capacity.HasValue)
        {
            query.Where("s.Capacity", ">=", filter.Capacity.Value);
        }

        if (filter is { StartDate: not null, EndDate: not null })
        {
            query.WhereNotExists(q => q
                .From("Reservation as res")
                .WhereColumns("res.SpaceId", "=", "s.SpaceId")
                .Where("res.StartDate", "<", filter.EndDate)
                .Where("res.EndDate", ">", filter.StartDate)
            );
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query.Where("s.Name", "LIKE", $"%{filter.Name}%");
        }

        if (filter.SpaceTypeId.HasValue)
        {
            query.Where("s.SpaceTypeId", filter.SpaceTypeId.Value);
        }

        if (filter.VenueTypeId.HasValue)
        {
            query.Where("v.VenueTypeId", filter.VenueTypeId.Value);
        }

        if (filter.AmenityIds is { Count: > 0 })
        {
            query.WhereIn("s.SpaceId",
                db.Query("SpaceAmenity")
                    .Select("SpaceId")
                    .WhereIn("AmenityId", filter.AmenityIds)
                    .GroupBy("SpaceId")
                    .Having("COUNT(DISTINCT AmenityId)", "=", filter.AmenityIds.Count)
            );
        }
        var result = await query.GetAsync<SpaceViewHolder>();
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

            // 3. VenueAddress
            var cities = await db.Query("VenueAddress")
                .Select("FullAddress")
                .Where("FullAddress", "LIKE", $"%{keyWord}%")
                .WhereNotNull("FullAddress")
                .Distinct()
                .Limit(3)
                .GetAsync<string>();
            suggestions.AddRange(cities.Where(x => !string.IsNullOrEmpty(x)));
        }
        catch (Exception)
        {
            // Log error if needed
            return [];
        }

        // Loại bỏ trùng lặp và giới hạn 10 kết quả
        return suggestions
            .Distinct()
            .Take(10)
            .OrderBy(s => s.Length)
            .ToList();
    }
}