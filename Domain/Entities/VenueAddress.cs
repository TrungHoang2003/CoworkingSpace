using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class VenueAddress
{
    [Key]
    public int VenueAddressId { get; set; }
    public string? Street { get; set; }
    public string? District { get; set; }
    public string? City { get; set; } 
    public string? FullAddress { get; set; } 
    
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}