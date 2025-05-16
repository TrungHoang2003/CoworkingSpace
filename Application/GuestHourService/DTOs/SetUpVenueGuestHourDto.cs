using System.ComponentModel.DataAnnotations;

namespace Application.GuestHourService.DTOs;

public class SetUpVenueGuestHourDto
{
    [Required] public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public bool IsOpen24Hours { get; set; }
    public bool IsClosed { get; set; }
}