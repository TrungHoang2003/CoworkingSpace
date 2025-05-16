using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class GuestHour
{
   [Key] public int GuestHourId { get; set; }
   
   public int VenueId { get; set; }
   [ForeignKey("VenueId")] public Venue? Venue { get; set; } 
   
   public DayOfWeek DayOfWeek { get; set; }
   public TimeSpan? StartTime { get; set; }
   public TimeSpan? EndTime { get; set; }
   public bool IsOpen24Hours { get; set; }
   public bool IsClosed { get; set; }
}