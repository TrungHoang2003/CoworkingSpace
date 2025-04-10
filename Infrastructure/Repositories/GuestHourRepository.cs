using Domain.Entites;
using Domain.Entities;
using Infrastructure.DbHelper;
using Infrastructure.Interfaces;
using static System.Enum;

namespace Infrastructure.Repositories;

public class GuestHourRepository(ApplicationDbContext dbContext)
    : GenericRepository<GuestHour>(dbContext), IGuestHourRepository
{
    public List<GuestHour> GenerateDefaultGuestHours(Venue venue)
    {
        var guestHours = new List<GuestHour>();

        foreach (var day in GetValues<DayOfWeek>())
        {
            var isWeekend = day is DayOfWeek.Saturday or DayOfWeek.Sunday;
            guestHours.Add(new GuestHour
            {
               IsClosed = isWeekend,
               Venue = venue,
                DayOfWeek = day,
                IsOpen24Hours = false,
                StartTime = isWeekend ? null: new TimeSpan(9, 0, 0),
                EndTime = isWeekend ? null : new TimeSpan(18, 0, 0)
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

}