namespace Application.DTOs;

public class VenueAddressDto
{
    public string? Street { get; set; }
    public string? District { get; set; }
    public string? City { get; set; } 
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}