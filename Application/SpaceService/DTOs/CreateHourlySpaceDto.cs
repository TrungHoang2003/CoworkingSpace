using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.SpaceService.DTOs;

public class BasicSpaceInfos
{
    public int SpaceId { get; set; }
    public int SpaceTypeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Capacity { get; set; }
    public decimal Amount { get; set; } // Số tiền tương ứng
    public List<IFormFile>? Images { get; set; }
    public IFormFile? Video { get; set; }
    public IFormFile? VirtualVideo{ get; set; }
    public List<int>? AmenityIds { get; set; }
}

public class SpaceImageDto
{
    public SpaceImageType Type;
    public IFormFile? Image;
}

public class SpaceVideoDto
{
    public IFormFile? Video;
    public IFormFile? VirtualVideo;
    public IFormFile? PdfFlyer;
}