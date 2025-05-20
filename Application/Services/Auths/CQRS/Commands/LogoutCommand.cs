using Application.SharedServices;
using Domain.Errors;
using Domain.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Auths.CQRS.Commands;

public sealed record LogoutCommand:IRequest<Result<LogoutResponse>>;

public class LogoutResponse
{
    public bool isLogin { get; set; }
}

public class LogoutCommandHandler(RedisService redis, IHttpContextAccessor httpContextAccessor) : IRequestHandler<LogoutCommand, Result<LogoutResponse>>
{
    public async Task<Result<LogoutResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.Items?["UserId"].ToString();
        if (userId == null) return Result<LogoutResponse>.Failure(AuthenErrors.NotLoggedIn);

        // Xóa token trong Redis
        
        var refreshRedisKey = $"refreshToken:{userId}";
        var result = await redis.DeleteValue(refreshRedisKey);
        if (!result) return Result<LogoutResponse>.Failure(RedisErrors.DeleteAccessTokenFailed);
        
        var accessRedisKey = $"accessToken:{userId}";
        var result2 = await redis.DeleteValue(accessRedisKey);
        if (!result2) return Result<LogoutResponse>.Failure(RedisErrors.DeleteRefreshTokenFailed);
        
        return Result<LogoutResponse>.Success(new LogoutResponse {
            isLogin = false
        });
    }
}