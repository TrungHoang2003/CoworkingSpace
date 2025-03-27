using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites;

public enum SpaceStatus
{
    Pending,
    Approved,
    Rejected
}

public class Space
{
    [Key] public int SpaceId { get; set; }

    public int CollectionId { get; set; } 
    [ForeignKey("CollectionId")] public Collection? Collection { get; set; }
    
    public int SpaceTypeId { get; set; }
    [ForeignKey("SpaceTypeId")] public SpaceType? SpaceType { get; set; }
    
    public int VenueId { get; set; }
    [ForeignKey("VenueId")] public Venue? Venue { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")] public decimal PricePerHour { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal PricePerMonth { get; set; }

    public int Capacity { get; set; }
    public SpaceStatus Status { get; set; } = SpaceStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<SpaceAmenity>? Amenities{ get; set; } // Many-to-Many with Amenities
    public ICollection<SpaceImage>? SpaceImages { get; set; } // One-to-Many with SpaceImages
    public ICollection<Reservation>? Reservations { get; set; } // One-to-Many with Reservations
    public ICollection<Review>? Reviews { get; set; } // One-to-Many with Reviews
}