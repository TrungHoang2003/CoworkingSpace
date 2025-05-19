using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Review
{
    [Key]
    public int ReviewId { get; set; }

    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public User? Customer { get; set; }

    public int SpaceId { get; set; }

    [ForeignKey("SpaceId")]
    public Space? WorkingSpace { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}