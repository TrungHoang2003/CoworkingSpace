using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs;

public class SignUpVenueDTO
{
    public int VenueTypeId { get; set; }
    [Required]public string PhoneNumber { get; set; }
    public IFormFile? UserAvatar { get; set; }
    public IFormFile? VenueLogo { get; set; }
    [Required]public string VenueName { get; set; }
    [Required]public string VenueDescription { get; set; }
    
    [Required]public string VenueStreet { get; set; }
    [Required]public string VenueCity { get; set; }
    [Required]public string VenueDistrict { get; set; }
    public string? VenueFloor{ get; set; }
    [Required]public decimal VenueLatitude { get; set; }
    [Required]public decimal VenueLongitude { get; set; }
}