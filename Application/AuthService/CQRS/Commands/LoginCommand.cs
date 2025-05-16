using Application.UserService.DTOs;
using Application.UserService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

namespace Application.AuthService.CQRS.Commands;

public sealed record LoginCommand(
    string UserName,
    string Password
    ) : IRequest<Result<LoginResponse>>;

public sealed record LoginResponse
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; } 
    public UserViewModel User { get; init; }
}

public class LoginCommandHandler(
    IUserRepository userRepository,
    JwtService jwtService,
    RedisService redisService)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByNameAsync(command.UserName);
        if (user == null)
            return Result<LoginResponse>.Failure(AuthenErrors.UserNotFound);

        var isPasswordValid = await userRepository.CheckPasswordAsync(user, command.Password);
        if (!isPasswordValid)
            return Result<LoginResponse>.Failure(AuthenErrors.WrongPassword);

        var roles = await userRepository.GetRolesAsync(user);
        var accessToken = jwtService.GenerateJwtToken(user, string.Join(",", roles));
        var refreshToken = jwtService.GenerateRefreshToken();

        var refreshKey = $"refreshToken:{user.Id}";
        var accessKey = $"accessToken:{user.Id}";

        try
        {
            await redisService.SetValue(refreshKey, refreshToken, TimeSpan.FromDays(jwtService.getRefreshTokenValidity()));
            await redisService.SetValue(accessKey, accessToken, TimeSpan.FromMinutes(jwtService.getAccessTokenValidity()));
        }
        catch (Exception ex)
        {
            return Result<LoginResponse>.Failure(new Error("Redis.SaveFailed", $"Failed to save tokens: {ex.Message}"));
        }
        
        var userViewModel = user.ToUserViewModel();
        
        return Result<LoginResponse>.Success(new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = userViewModel
        });
    }
}