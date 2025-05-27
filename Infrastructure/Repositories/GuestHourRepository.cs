using System.Runtime.CompilerServices;
using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using MySqlConnector;
using static System.Enum;

namespace Infrastructure.Repositories;

public interface IGuestHourRepository : IGenericRepository<GuestHour>
{
    Task<GuestHour?> GetGuestHoursByVenueId(int venueId);
}

public class GuestHourRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection)
    : GenericRepository<GuestHour>(dbContext), IGuestHourRepository
{
    public async Task<GuestHour?> GetGuestHoursByVenueId(int venueId)
    {
        var cnn = dbConnection.OpenConnection();
        try
        {
            var sql = $"select * from GuestHour where VenueId = {venueId}";
            var result = await cnn.QueryFirstOrDefaultAsync<GuestHour>(sql, new { VenueId = venueId });
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error getting guest hours", e);
        }
    }
}
