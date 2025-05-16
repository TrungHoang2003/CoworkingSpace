using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class GuestArrival
{
    public int Id { get; set; }
    public string? WelcomeMessage { get; set; }
    public string? EntryInformation { get; set; }
    public string? ParkingInformation { get; set; }
    public decimal? ParkingPrice { get; set; }
    public ParkingType? ParkingType{ get; set; }
    public string? WifiName { get; set; }
    public string? WifiPassword { get; set; }
}

public enum ParkingType
{
    FreeParkingOnsite = 0,
    FreeParkingOffsite = 1,
    PaidParkingOnsite = 2,
    PaidParkingOffsite = 3,
}