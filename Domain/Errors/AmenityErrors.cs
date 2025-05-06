using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record AmenityErrors
{
   public static readonly Error AmenityNotFound = new Error("AmenityError", "Amenity not found");
}