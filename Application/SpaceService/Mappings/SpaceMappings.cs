using Application.SpaceService.CQRS.Commands;
using Application.SpaceService.DTOs;
using Domain.Entities;

namespace Application.SpaceService.Mappings;

public static class SpaceMappings
{
    public static Space ToSpace(this SpaceInfoDto spaceInfoDto)
    {
        return new Space
        {
            ListingType = spaceInfoDto.ListingType,
            Quantity = spaceInfoDto.Quantity,
            Name = spaceInfoDto.Name,
            Description = spaceInfoDto.Description,
            Capacity = spaceInfoDto.Capacity,
            SpaceTypeId = spaceInfoDto.SpaceTypeId,
        };
    }
    
    public static Space ToSpace(this SpaceInfoDto spaceInfoDto, Space existingSpace)
    {
        existingSpace.Quantity = spaceInfoDto.Quantity;
        existingSpace.ListingType = spaceInfoDto.ListingType;
        existingSpace.Name = spaceInfoDto.Name;
        existingSpace.Description = spaceInfoDto.Description;
        existingSpace.Capacity = spaceInfoDto.Capacity;
        existingSpace.SpaceTypeId = spaceInfoDto.SpaceTypeId;
        return existingSpace;
    }
}

