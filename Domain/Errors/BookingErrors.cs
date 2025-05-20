using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record BookingErrors
{
   public static readonly Error SpaceNotAvailable = new Error("Lỗi đặt phòng", "Số lượng đặt vượt quá số lượng khả dụng");
   public static readonly Error ReservationNotFound= new Error("Lỗi đặt phòng", "Không tìm thấy thông tin đặt phòng");
}