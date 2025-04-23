using Infrastructure.Common;

namespace Domain.Errors;

public sealed record HolidayErrors
{
    public static readonly Error HolidayNotFound = new Error("HolidayError", "Holiday not found");
    public static readonly Error VenueHolidayNotFound = new Error("HolidayError", "Venue holiday not found");
}