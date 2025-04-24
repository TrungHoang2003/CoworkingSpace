using Infrastructure.Common;

namespace Domain.Errors;

public sealed record SpaceErrors
{
   public static readonly Error SpaceNotFound = new Error("SpaceError", "Space not found");
   public static readonly Error SpaceNotFoundInVenue = new Error("SpaceError", "Space not found in venue");
}