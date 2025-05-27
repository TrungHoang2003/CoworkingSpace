using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record PayOsErrors
{
   public static readonly Error InvalidSignature = new Error("Lỗi PayOs", "Chữ ký không hợp lệ");
   public static readonly Error ReservationNotFound = new Error("Lỗi PayOs", "Không tìm thấy thông tin đặt chỗ trùng có Id bằng OrderCode từ PayOs");
}