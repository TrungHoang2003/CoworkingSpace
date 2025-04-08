using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites;

public class SpaceImage
{
    [Key]
    public int ImageId { get; set; }
    public int SpaceId { get; set; }

    [ForeignKey("SpaceId")]
    public Space? WorkingSpace { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}