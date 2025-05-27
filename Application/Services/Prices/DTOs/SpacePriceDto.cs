namespace Application.Services.Prices.DTOs;

public class SpacePriceDto
{
    public decimal Amount { get; set; } // Amount per day
    public decimal? DiscountPercentage { get; set; }
    public decimal? SetupFee{ get; set; }
    
}