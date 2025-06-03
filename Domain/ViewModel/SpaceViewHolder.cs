using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class SpaceViewHolder
{
    public int SpaceId { get; set; }
    public string? ImageUrl { get; set; }
    public string? Name { get; set; }
    public bool IsLiked { get; set; } = false;
    public string? FullAddress { get; set; }
    [Column(TypeName = "decimal(10, 2)")] public decimal? Price { get; set; }
    [Column(TypeName = "decimal(10, 1)")] public decimal? Rate { get; set; }
}
