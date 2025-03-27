using System.ComponentModel.DataAnnotations;

namespace Domain.Entites;

public class Amenity
{
    [Key]
    public int AmenityId { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    // Navigation property
    public ICollection<SpaceAmenity>? WorkingSpaceAmenities { get; set; } // Many-to-Many with WorkingSpaces
}