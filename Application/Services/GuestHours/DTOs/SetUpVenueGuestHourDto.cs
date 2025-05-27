using System.ComponentModel.DataAnnotations;

namespace Application.Services.GuestHours.DTOs;

public class SetUpVenueGuestHourDto
{
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public bool Open24Hours { get; set; }
    public bool OpenOnSunday { get; set; }
    public bool OpenOnSaturday { get; set; }
    public bool IsClosed { get; set; }
}