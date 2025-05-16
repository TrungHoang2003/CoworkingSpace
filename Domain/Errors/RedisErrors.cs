using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record RedisErrors
{
   public static readonly Error DeleteRefreshTokenFailed = new Error("Redis Error", "Failed to delete refresh token from Redis");
   public static readonly Error DeleteAccessTokenFailed= new Error("Redis Error", "Failed to delete access token from Redis");
}