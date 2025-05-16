using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record BookingWindowErrors
{
   public static readonly Error BookingWindowNotFound = new Error("Lỗi Cửa Sổ Đặt Chỗ", "Không tìm thấy cửa sổ đặt chỗ");
}
