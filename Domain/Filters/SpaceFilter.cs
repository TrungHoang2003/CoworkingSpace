using Domain.Entities;

namespace Domain.Filters;

public class SpaceFilter
{
    public string? Name { get; set; }
    public ListingType? ListingType { get; set; }
    public int? Capacity { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? SpaceTypeId { get; set; }
    public int MinPrice { get; set; } = 0;
    public int MaxPrice { get; set; } = 10000000;
}