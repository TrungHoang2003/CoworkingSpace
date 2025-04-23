using Dapper;
using Domain.Entities;
using Infrastructure.DbHelper;
using Microsoft.Extensions.Configuration;
using static System.Enum;

namespace Infrastructure.Repositories;

public interface IGuestHourRepository : IGenericRepository<GuestHour>
{
    List<GuestHour> GenerateDefaultGuestHours(Venue venue);
    Task AddRangeAsync(List<GuestHour> guestHours);
    Task<List<GuestHour>> GetGuestHoursByVenueId(int venueId);
    void RemoveRange(List<GuestHour> guestHours);
}

public class GuestHourRepository(ApplicationDbContext dbContext, IConfiguration configuration)
    : GenericRepository<GuestHour>(dbContext), IGuestHourRepository
{
    public List<GuestHour> GenerateDefaultGuestHours(Venue venue)
    {
        var guestHours = new List<GuestHour>();

        foreach (var day in GetValues<DayOfWeek>())
        {
            guestHours.Add(new GuestHour
            {
                IsClosed = false,
                Venue = venue,
                DayOfWeek = day,
                IsOpen24Hours = false,
                StartTime =  new TimeSpan(9, 0, 0),
                EndTime =  new TimeSpan(18, 0, 0)
            });
        }
        return guestHours;
    }

    public async Task AddRangeAsync(List<GuestHour> guestHours)
    {
        try
        {
            await dbContext.GuestHour.AddRangeAsync(guestHours);
        }
        catch (Exception e)
        {
            throw new Exception("Error adding guest hours", e);
        }
    }

    public void RemoveRange(List<GuestHour> guestHours)
    {
        dbContext.GuestHour.RemoveRange(guestHours);
    }

    public async Task<List<GuestHour>> GetGuestHoursByVenueId(int venueId)
    {
        var cnn = new MySqlServer(configuration).OpenConnection();
        
        var sql = $"select * from GuestHour where VenueId = {venueId}";
        var result = await cnn.QueryAsync<GuestHour>(sql, new {VenueId = venueId});

        return result.ToList();
    }
}