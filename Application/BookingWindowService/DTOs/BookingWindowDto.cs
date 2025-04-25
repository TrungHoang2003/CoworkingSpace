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
    [Required] public List<int> SpaceIds { get; set; } = []; 
    
    public void Validate()
    {
        if (Unit != BookingTimeUnit.Days) return;
        
        if(MaxNoticeDays == null) 
            throw new Exception("If unit is days, max notice days must be provided.");
    }
}