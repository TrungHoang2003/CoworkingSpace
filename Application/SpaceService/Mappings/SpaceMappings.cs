using Application.SpaceService.CQRS.Commands;
using Application.SpaceService.DTOs;
using Domain.Entities;

namespace Application.SpaceService.Mappings;

public static class SpaceMappings
{
    public static Space ToSpace(this SpaceInfos spaceInfos, string? videoUrl, string? virtualVideoUrl, string? pdfFlyerUrl)
    {
        return new Space
        {
            Name = spaceInfos.Name,
            Description = spaceInfos.Description,
            Capacity = spaceInfos.Capacity,
            SpaceTypeId = spaceInfos.SpaceTypeId,
            VideoUrl = videoUrl,
            VirtualVideoUrl = virtualVideoUrl,
            PdfFlyerUrl = pdfFlyerUrl,
        };
    }
    
    public static Space ToSpace(this SpaceInfos spaceInfos, Space existingSpace, string? videoUrl, string? virtualVideoUrl, string? pdfFlyerUrl)
    {
        existingSpace.Name = spaceInfos.Name;
        existingSpace.Description = spaceInfos.Description;
        existingSpace.Capacity = spaceInfos.Capacity;
        existingSpace.SpaceTypeId = spaceInfos.SpaceTypeId;
        existingSpace.VideoUrl = videoUrl;
        existingSpace.VirtualVideoUrl = virtualVideoUrl;
        existingSpace.PdfFlyerUrl = pdfFlyerUrl;
        return existingSpace;
    }
}

