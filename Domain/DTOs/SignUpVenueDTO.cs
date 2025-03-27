using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs;

public class SignUpVenueDTO
{
    public int venueTypeId { get; set; }
    
    [Required]public string phoneNumber { get; set; }
    public IFormFile? userAvatar { get; set; }
    public IFormFile? VenueLogo { get; set; }
    public string? UserAvatarUrl { get; set; }
    
    public string? VenueLogoUrl { get; set; }
    [Required]public string VenueName { get; set; }
    [Required]public string VenueDescription { get; set; }
}