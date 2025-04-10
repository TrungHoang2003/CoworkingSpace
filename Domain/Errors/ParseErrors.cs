using Infrastructure.Common;

namespace Infrastructure.Errors;

public sealed record ParseErrors
{
   public static readonly Error ParseError = new ("Parse Error", "Cannot parse string to int ");
}