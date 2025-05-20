using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record SpaceErrors
{
   public static readonly Error SpaceImageNotFound = new Error("Space Error", "Space image not found");
   public static readonly Error SpaceAssetNotFound = new Error("Space Error", "Space asset not found");
   public static readonly Error SpaceNotFound = new Error("Space Error", "Space not found");
   public static readonly Error SpaceNotFoundInVenue = new Error("Space Error", "Space not found in venue");
   public static readonly Error SpaceTypeNotFound  = new Error("Space Error", "Space type not found");
   public static readonly Error NotDailySpaceType  = new Error("SpaceAssetType Error", "This space type is a Monthly space type");
   public static readonly Error NotMonthlySpaceType = new Error("SpaceAssetType Error", "This space type is a Daily space type");
}