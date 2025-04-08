using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites;

public class VenueImage
{
    [Key] public int VenueImageId { get; set; }
    
    public int VenueId { get; set; }
    [ForeignKey("VenueId")] public Venue Venue { get; set; }
    
    public string? ImageUrl { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}