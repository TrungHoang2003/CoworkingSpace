using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
public enum TimeUnit
{
    Day,
    Month,
}

public class Price
{
    [Key] public int Id{ get; set; }

    public TimeUnit TimeUnit{ get; set; }
    [Column(TypeName = "decimal(10, 2)")] public decimal? DiscountPercentage { get; set; }
    [Column(TypeName = "decimal(10, 2)")] public decimal Amount { get; set; } // Số tiền tương ứng
    [Column(TypeName = "decimal(10,2)")] public decimal? SetupFee{ get; set; }
}