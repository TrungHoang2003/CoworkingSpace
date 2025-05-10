using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record AmenityErrors
{
   public static readonly Error AmenityNotFound = new Error("Amenity Error", "Amenity not found");
   public static readonly Error SpaceAmenityNotFound = new Error("Amenity Error", "Space Amenity not found");
}