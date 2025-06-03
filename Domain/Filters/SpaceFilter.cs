using Domain.Entities;

namespace Domain.Filters;

public class SpaceFilter
{
    public string? Name { get; set; }
    public ListingType? Type { get; set; }
    public int? Capacity { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? SpaceTypeId { get; set; }
    public decimal MinPrice { get; set; } = 0;
    public decimal MaxPrice { get; set; } = 10000000;
    public int? VenueTypeId { get; set; }
    public List<int>? AmenityIds { get; set; }
}