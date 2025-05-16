using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Repositories;

public interface IVenueHolidayRepository : IGenericRepository<VenueHoliday>
{
    Task<VenueHoliday?> GetByVenueIdAndHolidayId(int holidayId, int venueId);
    Task GenerateDefaultHolidays(List<Holiday> holidays, Venue venue);
}

public class VenueHolidayRepository(ApplicationDbContext dbContext, DbConnection<MySqlConnection> dbConnection) : GenericRepository<VenueHoliday>(dbContext), IVenueHolidayRepository
{
    public async Task GenerateDefaultHolidays(List<Holiday> holidays, Venue venue)
    {
        foreach (var holiday in holidays)
        {
            var venueHoliday = new VenueHoliday
            {
                Venue = venue,
                HolidayId = holiday.HolidayId
            };
            await Create(venueHoliday);
        }
    }
    
    public async Task<VenueHoliday?> GetByVenueIdAndHolidayId(int holidayId, int venueId)
    {
        var cnn = dbConnection.OpenConnection();
        
        var sql = $"Select * from VenueHoliday where HolidayId = {holidayId} and VenueId = {venueId}";
        var result = await cnn.QueryFirstOrDefaultAsync<VenueHoliday>(sql, new {HolidayId = holidayId, VenueId = venueId});
        return result;
    }
}