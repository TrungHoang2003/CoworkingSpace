using System.Security.Claims;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.AuthService.CQRS.Commands;

public sealed record LogoutCommand:IRequest<Result>;

public class LogoutCommandHandler(RedisService redis, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.Items?["UserId"].ToString();
        if (userId == null) return Result<bool>.Failure(AuthenErrors.NotLoggedIn);

        // Xóa token trong Redis
        
        var refreshRedisKey = $"refreshToken:{userId}";
        var result = await redis.DeleteValue(refreshRedisKey);
        if (!result) return RedisErrors.DeleteAccessTokenFailed;
        
        var accessRedisKey = $"accessToken:{userId}";
        var result2 = await redis.DeleteValue(accessRedisKey);
        if (!result2) return RedisErrors.DeleteRefreshTokenFailed;
        
        return Result.Success();
    }
}