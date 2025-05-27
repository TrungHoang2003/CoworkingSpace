using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class GuestHour
{
   [Key] public int GuestHourId { get; set; }
   public TimeSpan? StartTime { get; set; }
   public TimeSpan? EndTime { get; set; }
   public bool Open24Hours { get; set; }
   public bool OpenOnSunday { get; set; }
   public bool OpenOnSaturday { get; set; }
   public bool IsClosed { get; set; }
}