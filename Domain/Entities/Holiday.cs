using System.Collections;

namespace Domain.Entities;

public class Holiday
{
   public int HolidayId { get; set; } 
   public string? Name { get; set; } 
   public DateTime Date{ get; set; }
   
   public ICollection<VenueHoliday>? Venues { get; set; }
}