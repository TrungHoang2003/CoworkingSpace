using Application.Services.GuestHours.DTOs;
using Domain.Entities;

namespace Application.Services.GuestHours.Mappings;

public static class GuestHourMapping
{
   public static GuestHour ToGuestHour(this SetUpVenueGuestHourDto setUpVenueGuestHourDto)
   {
      return new GuestHour
      {
         OpenOnSaturday = setUpVenueGuestHourDto.OpenOnSaturday,
         OpenOnSunday = setUpVenueGuestHourDto.OpenOnSunday,
         StartTime =setUpVenueGuestHourDto.StartTime,
         EndTime = setUpVenueGuestHourDto.EndTime,
         Open24Hours = setUpVenueGuestHourDto.Open24Hours,
      };
   }
}
