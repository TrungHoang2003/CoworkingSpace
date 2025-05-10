using Application.SpaceService.CQRS.Commands;
using Application.SpaceService.DTOs;
using Domain.Entities;

namespace Application.SpaceService.Mappings;

public static class SpaceMappings
{
    public static Space ToSpace(this SpaceInfos spaceInfos, string? VideoUrl = null, string? VirtualVideoUrl = null, string? PdfFlyerUrl = null)
    {
        return new Space
        {
            Name = spaceInfos.Name,
            Description = spaceInfos.Description,
            Capacity = spaceInfos.Capacity,
            SpaceTypeId = spaceInfos.SpaceTypeId,
            VideoUrl = VideoUrl,
            VirtualVideoUrl = VirtualVideoUrl,
            PdfFlyerUrl = PdfFlyerUrl,
        };
    }
    
    public static Space ToSpace(this SpaceInfos spaceInfos, Space existingSpace, string? VideoUrl = null, string? VirtualVideoUrl = null, string? PdfFlyerUrl = null)
    {
        existingSpace.Name = spaceInfos.Name;
        existingSpace.Description = spaceInfos.Description;
        existingSpace.Capacity = spaceInfos.Capacity;
        existingSpace.SpaceTypeId = spaceInfos.SpaceTypeId;
        existingSpace.VideoUrl = VideoUrl;
        existingSpace.VirtualVideoUrl = VirtualVideoUrl;
        existingSpace.PdfFlyerUrl = PdfFlyerUrl;
        return existingSpace;
    }
    
    public static SpaceImage ToSpaceImage(this SpaceImagesDto spaceImageDto, string? imageUrl = null)
    {
        return new SpaceImage
        {
            Type = spaceImageDto.Type,
            ImageUrl = imageUrl // Được truyền vào từ kết quả upload Cloudinary
        };
    }
    
}

