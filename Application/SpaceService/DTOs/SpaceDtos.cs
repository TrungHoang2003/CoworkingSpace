using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.SpaceService.DTOs;

public class SpaceInfos
{
    public int SpaceTypeId { get; set; }
    public ListingType? ListingType{ get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Capacity { get; set; }
    public int? Quantity { get; set; }
    public IFormFile? Video { get; set; }
    public IFormFile? VirtualVideo { get; set; }
    public IFormFile? PdfFlyer { get; set; }
}

public class SpaceImagesDto
{
    public SpaceImageType? Type { get; set; }
    public IFormFile? Image { get; set; }
}