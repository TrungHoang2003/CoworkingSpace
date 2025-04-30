using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class GuestHourDto
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