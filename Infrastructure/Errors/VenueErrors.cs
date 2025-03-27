using Infrastructure.Common;

namespace Infrastructure.Errors;

public sealed record VenueErrors
{
   public static readonly Error VenueNotFound = new("VenueError", "Venue not found");
   public static readonly Error UploadImageFailed = new("VenueError", "Upload image failed");
}