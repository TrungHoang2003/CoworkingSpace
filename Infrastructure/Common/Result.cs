namespace Infrastructure.Common;

public class Result(bool isSuccess, Error error)
{
   public bool IsSuccess { get;  } = isSuccess;
   public bool IsFailure => !IsSuccess;
   public Error Error { get; } = error;
   public static Result Success() => new(true, Error.None);
   public static Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
   public T Value { get; }

   private Result(T value) : base(true, Error.None)
   {
      Value = value;
   }

   private Result(Error error) : base(false, error)
   {
      Value = default!;
   }

   public static Result<T> Success(T value) => new(value);
   public new static Result<T> Failure(Error error) => new(error);
}