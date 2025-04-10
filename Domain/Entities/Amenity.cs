using System.ComponentModel.DataAnnotations;
using Domain.Entites;

namespace Domain.Entities;

public class Amenity
{
    [Key]
    public int AmenityId { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    // Navigation property
    public ICollection<SpaceAmenity>? SpaceAmenities { get; set; } 
}