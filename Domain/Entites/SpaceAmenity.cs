using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites;

public class SpaceAmenity
{
    [Key]
    public int WorkingSpaceAmenityId { get; set; }

    public int SpaceId { get; set; }

    [ForeignKey("SpaceId")]
    public Space Space { get; set; }

    public int AmenityId { get; set; }

    [ForeignKey("AmenityId")]
    public Amenity Amenity { get; set; }
}