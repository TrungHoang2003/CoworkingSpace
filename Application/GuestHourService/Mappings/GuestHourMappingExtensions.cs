using Application.GuestHourService.DTOs;
using Application.VenueService.CQRS.Commands;
using Application.VenueService.DTOs;
using Domain.Entities;

namespace Application.GuestHourService.Mappings;

public static class GuestHourMappingExtensions
{
   public static GuestHour ToGuestHour(this SetUpVenueGuestHourDto setUpVenueGuestHourDto)
   {
      return new GuestHour
      {
         DayOfWeek = setUpVenueGuestHourDto.DayOfWeek,
         StartTime =setUpVenueGuestHourDto.StartTime,
         EndTime = setUpVenueGuestHourDto.EndTime,
         IsOpen24Hours = setUpVenueGuestHourDto.IsOpen24Hours,
      };
   }
}
