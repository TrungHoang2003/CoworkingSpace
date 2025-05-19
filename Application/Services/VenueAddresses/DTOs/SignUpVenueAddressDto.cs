using System.ComponentModel.DataAnnotations;

namespace Application.VenueAddressService.DTOs;

public class SignUpVenueAddressDto
{
    [Required] public string Street { get; set; }
    [Required] public string District { get; set; }
    [Required] public string City { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}