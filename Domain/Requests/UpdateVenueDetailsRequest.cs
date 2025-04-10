using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs;

public class UpdateVenueDetailsRequest
{
    [Required]public int VenueId { get; set; }
    public string? VenueName { get; set; }
    public IFormFile? VenueLogo { get; set; }
    
    public string? VenueDescription { get; set; }
    public string? VenueStreet { get; set; }
    public string? VenueDistrict { get; set; }
    public string? VenueCity { get; set; }
    public decimal? VenueLatitude { get; set; }
    public decimal? VenueLongitude { get; set; }
    public string? VenueFloor { get; set; }
}