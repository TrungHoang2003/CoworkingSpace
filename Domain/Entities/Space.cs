using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.Entites;

public enum SpaceStatus
{
    Pending,
    Approved,
    Rejected
}

public class Space
{
    [Key] public int SpaceId { get; set; }

    // Loại không gian
    public int SpaceTypeId { get; set; }
    [ForeignKey("SpaceTypeId")] public SpaceType? SpaceType { get; set; }
    
    // Văn phòng, địa điểm 
    public int VenueId { get; set; }
    [ForeignKey("VenueId")] public Venue? Venue { get; set; }
    
    // Khung giờ đặt tối thiểu và tối đa
    [ForeignKey("BookingWindowId")] public BookingWindow? BookingWindow { get; set; }
    public int? BookingWindowId { get; set; }
    
    // Thông tin chi tiết của không gian làm việc
    public string? Name { get; set; }
    public string? Description { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal PricePerHour { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal PricePerMonth { get; set; }
    public int Capacity { get; set; }
    public SpaceStatus Status { get; set; } = SpaceStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Quan hệ
    public ICollection<SpaceCollection>? Collections { get; set; } // Thuộc 1 hoặc nhiều bộ sưu tập
    public ICollection<SpaceAmenity>? Amenities{ get; set; } // Các dịch vụ tiện ích
    public ICollection<SpaceImage>? SpaceImages { get; set; } // Các ảnh mô tả
    public ICollection<Reservation>? Reservations { get; set; } // 
    public ICollection<Review>? Reviews { get; set; } // Các đánh giá không gian
}