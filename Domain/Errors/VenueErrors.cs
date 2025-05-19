using Domain.ResultPattern;
using Domain.ViewModel;

namespace Domain.Errors;

public sealed record VenueErrors
{
   public static readonly Error VenueNotFound = new("VenueError", "Venue not found");
   public static readonly Error VenueAddressNotFound = new("VenueError", "VenueAddress not found");
   public static readonly Error UploadImageFailed = new("VenueError", "Upload image failed");
   public static readonly Error VenueTypeNotFound = new("VenueType", "VenueType not found");
}