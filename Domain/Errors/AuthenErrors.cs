using Infrastructure.Common;

namespace Infrastructure.Errors;

public sealed record AuthenErrors
{
   public static readonly Error PasswordNotFound = new Error("AuthenticationError", "Password not found");
   public static readonly Error UserNotFound = new Error("AuthenticationError", "User not found");
   public static readonly Error WrongPassword = new Error("AuthenticationError", "Wrong password"); 
}