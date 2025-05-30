using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class VenueAddress
{
    [Key] public int VenueAddressId { get; set; }
    public string? District { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; } 
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? FullAddress { get; set; } 
    
    public void UpdateFullAddress()
    {
        FullAddress = $"{Street}, {District}, {City}";
    }
}