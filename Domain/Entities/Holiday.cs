using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Entities;

public class Holiday
{
   [Key]
   public int HolidayId { get; set; } 
   public string? Name { get; set; } 
   
   public ICollection<VenueHoliday>? Venues { get; set; }
}

public class HolidayDate
{
   [Key]
   public int HolidayDateId { get; set; }
   
   public int HolidayId { get; set; }
   [ForeignKey("HolidayId")] public Holiday Holiday { get; set; }
   
   public DateTime StartDate { get; set; }
   public DateTime? EndDate { get; set; }
    
}

public class VenueHoliday
{
   [Key]public int Id { get; set; } 
   
   public int VenueId { get; set; }
   [ForeignKey("VenueId")] public Venue Venue { get; set; }
   
   public int HolidayId { get; set; }
   [ForeignKey("HolidayId")] Holiday Holiday { get; set; } 
   public bool IsObserved { get; set; }
}
