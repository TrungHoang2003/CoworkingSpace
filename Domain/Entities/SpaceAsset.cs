using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entites;

namespace Domain.Entities;

public class SpaceAsset
{
    [Key]
    public int Id{ get; set; }

    public int SpaceId { get; set; }
    [ForeignKey("SpaceId")] Space? Space { get; set; }

    public string Url { get; set; }
    public SpaceAssetType? Type{ get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}

public enum SpaceAssetType
{
    Workspace = 0,
    CommonArea = 1,
    Pdf = 2,
    Video = 3,
    VirtualVideo = 4
}