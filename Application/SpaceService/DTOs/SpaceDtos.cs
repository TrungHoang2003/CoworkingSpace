using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.SpaceService.DTOs;

public class SpaceInfos
{
    public int SpaceTypeId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Capacity { get; set; }
    public IFormFile? Video;
    public IFormFile? VirtualVideo;
    public IFormFile? PdfFlyer;
}

public class SpaceImagesDto
{
    public int? ImageId{ get; set; }
    public SpaceImageType Type;
    public IFormFile Image;
    public bool isCreate;
}

