using Infrastructure.Common;

namespace Domain.Errors;

public sealed record GuestHourErrors
{
    public static readonly Error GuestHourNotFound = new("GuestHourError", "Guest hour not found");
}