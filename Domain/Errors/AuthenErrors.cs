using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record AuthenErrors
{
   public static readonly Error PasswordNotFound = new Error("Lỗi Xác Thực", "Không tìm thấy mật khẩu");
   public static readonly Error UserNotFound = new Error("Lỗi Xác Thực", "Không tìm thấy người dùng");
   public static readonly Error UserIdNotFoundInJwt = new Error("Lỗi Xác Thực", "Không tìm thấy Id người dùng trong Jwt");
   public static readonly Error NotLoggedIn = new Error("Lỗi Xác Thực", "Người dùng chưa đăng nhập");
   public static readonly Error WrongPassword = new Error("Lỗi Xác Thực", "Sai mật khẩu"); 
}
