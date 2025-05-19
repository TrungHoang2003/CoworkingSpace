using Application.VenueService.DTOs;
using Domain.Entities;

namespace Application.VenueService.Mappings;

public static class GuestArrivalMapping
{
   public static GuestArrival ToGuestArrival(this GuestArrivalDto dto)
   {
      return new GuestArrival
      {
         WelcomeMessage = dto.WelcomeMessage,
         EntryInformation = dto.EntryInformation,
         ParkingInformation = dto.ParkingInformation,
         ParkingPrice = dto.ParkingPrice,
         ParkingType = dto.ParkingType,
         WifiName = dto.WifiName,
         WifiPassword = dto.WifiPassword
      };
   }
}