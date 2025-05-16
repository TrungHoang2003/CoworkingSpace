using System.ComponentModel.DataAnnotations;
using Domain.Entites;

namespace Domain.Entities;

public class BookingWindow
{
   [Key]public int BookingWindowId { get; set; }
   public int MinNotice { get; set; }
   public int? MaxNoticeDays { get; set; }
    public bool? DisplayOnCalendar { get; set; } = false;
   
   public BookingTimeUnit Unit { get; set; }
   
   public ICollection<Space>? Spaces { get; set; } // một BookingWindow có thể áp dụng cho nhiều Space
}

public enum BookingTimeUnit
{
   Hours, // 0
   Days   // 1
}