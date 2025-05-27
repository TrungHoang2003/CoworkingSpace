using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record CollectionErrors
{
   public static readonly Error CollectionNotFound = new Error("Collection Error", "Bộ sưu tập không tồn tại");
   public static readonly Error SpaceNotInCollection = new Error("Collection Error", "Không gian không có trong bộ sưu tập");
}