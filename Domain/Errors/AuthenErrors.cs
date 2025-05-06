using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record AuthenErrors
{
   public static readonly Error PasswordNotFound = new Error("Authentication Error", "Password not found");
   public static readonly Error UserNotFound = new Error("Authentication Error", "User not found");
   public static readonly Error NotLoggedIn = new Error("Authentication Error", "User not logged in");
   public static readonly Error WrongPassword = new Error("Authentication Error", "Wrong password"); 
}