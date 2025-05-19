using Domain.Entities;

namespace Application.Services.Spaces.DTOs;

public class SpaceImageDto
{
    public string? Image { get; set; }
    public SpaceAssetType? Type { get; set; }
}