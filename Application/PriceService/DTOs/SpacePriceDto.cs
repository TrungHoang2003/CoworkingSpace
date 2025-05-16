using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Application.PriceService.DTOs;

public class SpacePriceDto
{
    public decimal Amount { get; set; } // Amount per day
    public TimeUnit TimeUnit{ get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? SetupFee{ get; set; }
    
}