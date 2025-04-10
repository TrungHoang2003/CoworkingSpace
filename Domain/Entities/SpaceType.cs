using System.ComponentModel.DataAnnotations;
using Domain.Entites;

namespace Domain.Entities;

public class SpaceType
{
    [Key]
    public int SpaceTypeId { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    // Navigation property
    public ICollection<Space>? WorkingSpaces { get; set; } // One-to-Many with WorkingSpaces
}