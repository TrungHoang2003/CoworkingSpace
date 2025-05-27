using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record SpaceErrors
{
   public static readonly Error SpaceImageNotFound = new Error("Space Error", "Space image not found");
   public static readonly Error SpaceAssetNotFound = new Error("Space Error", "Space asset not found");
   public static readonly Error SpaceNotFound = new Error("Space Error", "Space not found");
   public static readonly Error SpaceNotFoundInVenue = new Error("Space Error", "Space not found in venue");
   public static readonly Error SpaceTypeNotFound  = new Error("Space Error", "Space type not found");
   public static readonly Error NotNormalSpaceType  = new Error("SpaceAssetType Error", "Your are creating a Month Only space, but this space type is a normal space type");
   public static readonly Error NotMonthOnlySpaceType = new Error("SpaceAssetType Error", "Your are creating a normal space, but this space type is a month only space type");
   public static readonly Error PriceNotFound = new Error("Space Error", "Không gian chưa thiết lập giá");
}