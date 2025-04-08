using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class GuestHour
{
   [Key] public int GuestHourId { get; set; }
   public int VenueId { get; set; }
   public DayOfWeek DayOfWeek { get; set; }
   public TimeSpan StartTime { get; set; }
   public TimeSpan EndTime { get; set; }
   public bool IsOpen24Hours { get; set; }
   public bool IsClosed { get; set; }
}