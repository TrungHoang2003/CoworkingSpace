using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Amenity
{
    [Key]
    public int AmenityId { get; set; }
    public string? LogoUrl { get; set; } 
    public string? Name { get; set; }
    public string? Description { get; set; }

    // Navigation property
    public ICollection<SpaceAmenity>? SpaceAmenities { get; set; } 
}

public class SpaceAmenity
{
    [Key]
    public int SpaceAmenityId { get; set; }
    public int SpaceId { get; set; }
    public bool? Availability{ get; set; }
    public bool? HasFee{ get; set; }
    
    [ForeignKey("SpaceId")]
    public Space Space { get; set; }

    public int AmenityId { get; set; }

    [ForeignKey("AmenityId")]
    public Amenity Amenity { get; set; }
}

