namespace Domain.DTOs;

public class UpdateHolidayRequest
{
    public int VenueId { get; set; }
    public List<int> HolidayIds { get; set; } = [];
}