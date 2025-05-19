using Domain.Entities;

namespace Application.Services.Spaces.DTOs;

public class SpaceInfoDto
{
    public int SpaceTypeId { get; set; }
    public ListingType ListingType{ get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Capacity { get; set; }
    public int? Quantity { get; set; }
}