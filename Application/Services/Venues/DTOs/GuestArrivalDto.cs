using Domain.Entities;

namespace Application.VenueService.DTOs;

public class GuestArrivalDto
{
    public string? WelcomeMessage { get; set; }
    public string? EntryInformation { get; set; }
    public string? ParkingInformation { get; set; }
    public decimal? ParkingPrice { get; set; }
    public ParkingType? ParkingType { get; set; }
    public string? WifiName { get; set; }
    public string? WifiPassword { get; set; }
}