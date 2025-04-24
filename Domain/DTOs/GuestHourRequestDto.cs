using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Domain.DTOs;

public class GuestHourRequestDto
{
   [Required] public DayOfWeek DayOfWeek { get; set; }
   public TimeSpan? StartTime { get; set; }
   public TimeSpan? EndTime { get; set; }
    public bool IsOpen24Hours { get; set; }
    public bool IsClosed { get; set; }

   public void Normalize()
   {
      if (IsClosed || IsOpen24Hours)
      {
         StartTime = null;
         EndTime = null;
      }
      else if (StartTime == null || EndTime == null)
         throw new Exception("Start and end times must be provided if not closed or open 24 hours.");
   }
}

public class UpdateGuestHoursRequest
{
   [Required] public int VenueId { get; set; }
   [Required] public List<GuestHourRequestDto> GuestHours { get; set; } = [];

   public void Validate()
   {
      if (GuestHours.Count != 7 || GuestHours.Select(x => x.DayOfWeek).Distinct().Count() != 7)
         throw new Exception("Guest hours must be provided for all 7 days of the week.");

      foreach (var guestHour in GuestHours)
         guestHour.Normalize();
   }
}
