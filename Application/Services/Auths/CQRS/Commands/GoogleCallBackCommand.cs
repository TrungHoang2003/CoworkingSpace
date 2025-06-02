using Application.SharedServices;
using Application.UserService.Mappings;
using Domain.ResultPattern;
using Domain.ViewModel;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Auths.CQRS.Commands;

public sealed record GoogleCallbackCommand(string Code) : IRequest<Result<GoogleCallBackResponse>>;

public sealed record GoogleCallBackResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required string ReturnUrl { get; init; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
};

public class GoogleCallbackCommandHandler(
    GoogleAuthService googleAuthService,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    RedisService redisService,
    JwtService jwtService)
    : IRequestHandler<GoogleCallbackCommand, Result<GoogleCallBackResponse>>
{
    public async Task<Result<GoogleCallBackResponse>> Handle(GoogleCallbackCommand command, CancellationToken cancellationToken)
    {
        var payloadResult = await googleAuthService.ValidateGoogleCodeAsync(command.Code);
        if (!payloadResult.IsSuccess)
            return Result<GoogleCallBackResponse>.Failure(payloadResult.Error);

        var payload = payloadResult.Value;
        var user = await userRepository.FindByEmailAsync(payload.Email);

        if (user == null)
        {
            var registerCommand = new GoogleRegisterCommand(
                payload.Email,
                payload.Name,
                payload.Email,
                payload.Picture);
            var registerResult = await new GoogleRegisterCommandHandler(userRepository, roleRepository)
                .Handle(registerCommand, cancellationToken);
            if (!registerResult.IsSuccess)
                return Result<GoogleCallBackResponse>.Failure(registerResult.Error);

            user = await userRepository.FindByEmailAsync(payload.Email);
            if (user == null)
                return Result<GoogleCallBackResponse>.Failure(
                    new Error("User.CreateFailed", "Failed to create user after registration"));
        }

        var roles = await userRepository.GetRolesAsync(user);
        var refreshToken = jwtService.GenerateRefreshToken();
        var jwtToken = jwtService.GenerateJwtToken(user, string.Join(",", roles));

        var refreshKey = $"refreshToken:{user.Id}";
        var accessKey = $"accessToken:{user.Id}";

        try
        {
            await redisService.SetValue(refreshKey, refreshToken,
                TimeSpan.FromDays(jwtService.getRefreshTokenValidity()));
            await redisService.SetValue(accessKey, jwtToken, TimeSpan.FromMinutes(jwtService.getAccessTokenValidity()));
        }
        catch (Exception ex)
        {
            return Result<GoogleCallBackResponse>.Failure(new Error("Redis.SaveFailed", $"Failed to save tokens: {ex.Message}"));
        }
        var googleResponse = new GoogleCallBackResponse
        {
            AccessToken = jwtToken,
            RefreshToken = refreshToken,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            ReturnUrl = "http://localhost:3000/home"
        };
        return Result<GoogleCallBackResponse>.Success(googleResponse);
    }
}