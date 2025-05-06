using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ExceptionRuleDay
{
    [Key]
    public int Id { get; set; }
    public int ExceptionRuleId { get; set; } 
    public DayOfWeek DayOfWeek { get; set; }

    // Quan hệ
    public ExceptionRule ExceptionRule { get; set; } 
}