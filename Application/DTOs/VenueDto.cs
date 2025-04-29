using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.VenueService.DTOs;

public class HostInformationDto
{
    public string? PhoneNumber { get; set; }
    public IFormFile? UserAvatar { get; set; }
}

public class VenueAddressDto
{
    public string? Street { get; set; }
    public string? District { get; set; }
    public string? City { get; set; } 
    public string? FullAddress { get; set; } 
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}

public class UpdateVenueRequest
{
    public int VenueId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Floor { get; set; }
    public VenueAddressDto? Address { get; set; }
}

public class SignUpVenueRequest
{
    [Required] public int VenueTypeId { get; set; }
    public IFormFile? Logo { get; set; }
    
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }
    
    [Required] public VenueAddressDto Address { get; set; }
    [Required] public HostInformationDto HostInformation { get; set; }
}
