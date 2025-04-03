using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entites;

public class Venue
{
    [Key] public int VenueId { get; set; }

    public int HostId { get; set; }
    [ForeignKey("HostId")] public User? Host { get; set; }
    public int VenueTypeId { get; set; }
    [ForeignKey("VenueTypeId")] public VenueType Type { get; set; }

    public int? VenueAddressId { get; set; }
    [ForeignKey("VenueAddressId")] public VenueAddress Address { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? VenueLogoUrl { get; set; }
    
    public string? Floor { get; set; }

    public ICollection<VenueImage>? VenueImages { get; set; }
    public ICollection<Space> Spaces { get; set; }
}