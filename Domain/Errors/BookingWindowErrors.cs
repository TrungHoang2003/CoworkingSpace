using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record BookingWindowErrors
{
   public static readonly Error BookingWindowNotFound = new Error("Booking Window Error", "BookingWindow not found");
}