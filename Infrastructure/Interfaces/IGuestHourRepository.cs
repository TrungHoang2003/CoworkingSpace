using Domain.Entites;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IGuestHourRepository : IGenericRepository<GuestHour>
{
    List<GuestHour> GenerateDefaultGuestHours(Venue venue);
    Task AddRangeAsync(List<GuestHour> guestHours);
}

