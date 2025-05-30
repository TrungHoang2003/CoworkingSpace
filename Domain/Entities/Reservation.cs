using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entites;

namespace Domain.Entities;

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }

    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public User? Customer { get; set; }

    public int SpaceId { get; set; }

    [ForeignKey("SpaceId")]
    public Space? Space { get; set; }

    public DateTime ReservationDate { get; set; }

    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int NumPeople { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Payment? Payment { get; set; } // One-to-One with Payment
}