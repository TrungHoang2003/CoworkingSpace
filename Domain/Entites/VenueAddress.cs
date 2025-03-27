using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites;

public class VenueAddress
{
    [Key]
    public int VenueAddressId { get; set; }
    
    public string StreetAddress { get; set; }
    public string District { get; set; }
    public string City { get; set; } 
    
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}