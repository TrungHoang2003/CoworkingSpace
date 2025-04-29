using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entites;

namespace Domain.Entities;

public class SpaceImage
{
    [Key]
    public int ImageId { get; set; }

    public int SpaceId { get; set; }
    [ForeignKey("SpaceId")] Space? Space { get; set; }

    public string? ImageUrl { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}