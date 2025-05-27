using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Price
{
    [Key] public int Id{ get; set; }
    [Column(TypeName = "decimal(10, 2)")] public decimal? DiscountPercentage { get; set; }
    [Column(TypeName = "decimal(10, 2)")] public decimal Amount { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal? SetupFee{ get; set; }
}