using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs;

public class BookingWindowDto
{
    public int VenueId { get; set; }
    [Required] public int MinNotice { get; set; }
    public int? MaxNoticeDays { get; set; }
    [Required] public BookingTimeUnit Unit { get; set; }
    public bool? DisplayOnCalendar { get; set; } = false;
    public bool ApplyAll { get; set; } = true;
    public List<int>? SpaceIds { get; set; } = []; 
    
    public void Validate()
    {
        if (Unit != BookingTimeUnit.Days) return;
        
        if(MaxNoticeDays == null) 
            throw new Exception("If unit is days, max notice days must be provided.");

        if (ApplyAll) return;
        if(SpaceIds == null || SpaceIds.Count == 0)
            throw new Exception("At least 1 space is required when ApplyAll is false.");
    }
}

public class UpdateBookingWindowRequest: BookingWindowDto
{
    public int BookingWindowId { get; set; }
}

public class AddBookingWindowRequest: BookingWindowDto
{
}