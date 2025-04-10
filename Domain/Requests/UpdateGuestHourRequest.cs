using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entites;
using Domain.Entities;

namespace Domain.DTOs;

public class UpdateGuestHourRequest
{
   [Key] public int GuestHourId { get; set; }
   public DayOfWeek DayOfWeek { get; set; }
   public TimeSpan StartTime { get; set; }
   public TimeSpan EndTime { get; set; }
   public bool IsOpen24Hours { get; set; }
   public bool IsClosed { get; set; }
   
   public int VenueId { get; set; }
   [ForeignKey("VenueId")] public Venue? Venue { get; set; }
}