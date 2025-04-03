using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites;

public class VenueAddress
{
    [Key]
    public int VenueAddressId { get; set; }
    
    public int VenueId { get; set; }
    [ForeignKey("VenueId")] public Venue Venue { get; set; }
    
    public string? Street { get; set; }
    public string? District { get; set; }
    public string? City { get; set; } 
    public string? FullAddress { get; set; } 
    
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}