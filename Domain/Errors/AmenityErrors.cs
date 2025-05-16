using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record AmenityErrors
{
   public static readonly Error AmenityNotFound = new Error("Lỗi Tiện Ích", "Không tìm thấy tiện ích");
   public static readonly Error SpaceAmenityNotFound = new Error("Lỗi Tiện Ích", "Không tìm thấy tiện ích không gian");
}
