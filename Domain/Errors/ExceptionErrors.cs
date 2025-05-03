using Infrastructure.Common;

namespace Domain.Errors;

public sealed record class ExceptionErrors
{
   public static readonly Error ExceptionNotFound = new Error("Exception Error", "Exception not found");
}