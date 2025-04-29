namespace Application.HolidayService.DTOs;

public class UpdateHolidayRequest
{
    public int VenueId { get; set; }
    public List<int> HolidayIds { get; set; } = [];
}