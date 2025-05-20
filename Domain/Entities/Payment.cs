using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public enum PaymentMethod
{
    VnPay,
    Momo,
    CreditCard
}

public enum PaymentStatus
{
    Pending,
    Success,
    Failed
}

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int ReservationId { get; set; }
    [ForeignKey("ReservationId")] public Reservation? Reservation { get; set; }

    public int CustomerId { get; set; }
    [ForeignKey("CustomerId")] public User? Customer { get; set; }
    
    public decimal TotalPrice { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}