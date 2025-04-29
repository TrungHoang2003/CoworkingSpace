using System.ComponentModel.DataAnnotations;
using Domain.Entites;

namespace Domain.Entities;

public class ExceptionRule
{
    [Key] public int ExceptionId { get; set; }
    public ExceptionUnit Unit { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? Description { get; set; }
    public List<DayOfWeek>? Days { get; set; }

    public ICollection<Space>? Spaces { get; set; }
}

public enum ExceptionUnit
{
    DaysOfWeek,
    DateRange
}