using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Domain.Entities;

public class ExceptionRule
{
   [Key] public int ExceptionId { get; set; }
   public ExceptionUnit Unit { get; set; }
   
   public DateTime StartDate { get; set; }
   public DateTime EndDate { get; set; }
   
   public DayOfWeek [] Days { get; set; }
}

public enum ExceptionUnit
{
   Days,
   Hours
}