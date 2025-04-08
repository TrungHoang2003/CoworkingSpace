using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.Entites;

public class Venue
{
    [Key] public int VenueId { get; set; }

    // Người sở hữu văn phòng
    public int HostId { get; set; }
    [ForeignKey("HostId")] public User? Host { get; set; }
    
    // Loại văn phòng
    public int VenueTypeId { get; set; }
    [ForeignKey("VenueTypeId")] public VenueType Type { get; set; }
 
    // Địa chỉ văn phòng
    public int VenueAddressId { get; set; }
    [ForeignKey("VenueAddressId")] public VenueAddress Address { get; set; }

    // Thông tin văn phòng
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? VenueLogoUrl { get; set; }
    
    public string? Floor { get; set; }

    public ICollection<VenueImage>? VenueImages { get; set; } // Hình ảnh văn phòng
    public ICollection<Space> Spaces { get; set; } // Danh sách không gian trong văn phòng
    public ICollection<GuestHour> GuestHours { get; set; } // Giờ cho thuê
}