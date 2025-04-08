using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entites;

namespace Domain.Entities.WorkingSpace;

public class SpaceObservedHoliday
{
    [Key]public int Id { get; set; }
    
    public int SpaceId { get; set; }
    [ForeignKey("SpaceId")] Space Space { get; set; }
    
    public int HolidayId { get; set; }
    [ForeignKey("HolidayId")] Holiday Holiday { get; set; }
}