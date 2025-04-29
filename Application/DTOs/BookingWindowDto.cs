using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entites;
using Domain.Entities;

namespace Application.BookingWindowService.DTOs;

public class SetBookingWindowRequest
{
    [Required] public int MinNotice { get; set; }
    public int? MaxNoticeDays { get; set; }
    [Required] public BookingTimeUnit Unit { get; set; }

    [Required] public int VenueId { get; set; }
    public List<int>? SpaceIds { get; set; } = []; 
    public bool ApplyAll { get; set; } = true;
    
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