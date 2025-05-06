namespace Domain.ResultPattern;

public sealed record Error(string? code, string? description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
   public static implicit operator Result(Error error) => Result.Failure(error);
}
