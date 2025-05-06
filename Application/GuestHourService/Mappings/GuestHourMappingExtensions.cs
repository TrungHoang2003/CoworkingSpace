using Application.VenueService.CQRS.Commands;
using Domain.Entities;

namespace Application.GuestHourService.Mappings;

public static class GuestHourMappingExtensions
{
   public static GuestHour ToGuestHour(this SetUpVenueCommand command)
   {
      return new GuestHour
      {
         StartTime = command.StartTime,
         EndTime = command.EndTime,
         ExceptionRuleDays = command.ExceptionRuleDays.Select(day => new ExceptionRuleDay
         {
            DayOfWeek = day
         }).ToList()
      };
   }
}