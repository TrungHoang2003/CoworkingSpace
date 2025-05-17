using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.SpaceService.DTOs;

public class SpaceInfoDto
{
    public int SpaceTypeId { get; set; }
    public ListingType ListingType{ get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Capacity { get; set; }
    public int? Quantity { get; set; }
}

public class SpaceAssetDto
{
    public string? Video { get; set; }
    public string? VirtualVideo { get; set; }
    public string? PdfFlyer { get; set; }
}

public class SpaceImageDto
{
    public string? Image { get; set; }
    public SpaceAssetType? Type { get; set; }
}