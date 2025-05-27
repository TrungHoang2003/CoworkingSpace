using Application.Services.Spaces.DTOs;
using Domain.Entities;

namespace Application.Services.Spaces.Mappings;

public static class SpaceMappings
{
    public static Space ToSpace(this SpaceInfoDto spaceInfoDto)
    {
        return new Space
        {
            Name = spaceInfoDto.Name,
            Description = spaceInfoDto.Description,
            Capacity = spaceInfoDto.Capacity,
            SpaceTypeId = spaceInfoDto.SpaceTypeId,
        };
    }
    
    public static Space ToSpace(this SpaceInfoDto spaceInfoDto, Space existingSpace)
    {
        existingSpace.Name = spaceInfoDto.Name;
        existingSpace.Description = spaceInfoDto.Description;
        existingSpace.Capacity = spaceInfoDto.Capacity;
        existingSpace.SpaceTypeId = spaceInfoDto.SpaceTypeId;
        return existingSpace;
    }
}

