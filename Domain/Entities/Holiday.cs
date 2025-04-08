using Domain.Entities.WorkingSpace;

namespace Domain.Entities;

public class Holiday
{
   public int HolidayId { get; set; } 
   public string? Name { get; set; } 
   public DateTime Date{ get; set; }
   public bool IsObserved { get; set; }
   
   public ICollection<SpaceObservedHoliday>? Spaces{ get; set; }
}