using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ExceptionRule
{
    [Key] public int ExceptionId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? Description { get; set; }
    public bool IsClosed24Hours { get; set; }
    
    public ICollection<Space>? Spaces { get; set; }
}