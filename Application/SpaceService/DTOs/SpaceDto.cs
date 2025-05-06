using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.SpaceService.DTOs;


public class SetHourlySpaceRequest
{
    public int SpaceId { get; set; }
    [Required] public int VenueId { get; set; }
    [Required] public int SpaceTypeId { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Capacity { get; set; }
    [Required] public decimal Amount { get; set; } // Số tiền tương ứng
    public List<IFormFile>? Images { get; set; }
    public IFormFile? Video { get; set; }
    public IFormFile? VirtualVideo{ get; set; }
    public List<int>? AmenityIds { get; set; }
}