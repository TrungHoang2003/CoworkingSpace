using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class VenueHoliday
{
   [Key]public int Id { get; set; } 
   
   public int VenueId { get; set; }
   [ForeignKey("VenueId")] Venue Venue { get; set; }
   
   public int HolidayId { get; set; }
   [ForeignKey("HolidayId")] Holiday Holiday { get; set; }

   public bool IsObserved { get; set; } = true;
}