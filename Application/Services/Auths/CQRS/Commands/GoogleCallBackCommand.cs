using Application.SharedServices;
using Application.UserService.Mappings;
using Domain.ResultPattern;
using Domain.ViewModel;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Auths.CQRS.Commands;

public sealed record GoogleCallbackCommand(string Code) : IRequest<Result<string>>;

public class GoogleCallbackCommandHandler(
    GoogleAuthService googleAuthService,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    RedisService redisService,
    JwtService jwtService)
    : IRequestHandler<GoogleCallbackCommand, Result<string>>
{
    public async Task<Result<string>> Handle(GoogleCallbackCommand command, CancellationToken cancellationToken)
    {
        var payloadResult = await googleAuthService.ValidateGoogleCodeAsync(command.Code);
        if (!payloadResult.IsSuccess)
            return Result<string>.Failure(payloadResult.Error);

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
                return Result<string>.Failure(registerResult.Error);
            
            user = await userRepository.FindByEmailAsync(payload.Email);
            if (user == null)
                return Result<string>.Failure(new Error("User.CreateFailed", "Failed to create user after registration"));
        }

        var roles = await userRepository.GetRolesAsync(user);
        var jwtToken = jwtService.GenerateJwtToken(user, string.Join(",", roles));
        var refreshToken = jwtService.GenerateRefreshToken();

        var refreshKey = $"refreshToken:{user.Id}";
        var accessKey = $"accessToken:{user.Id}";

        try
        {
            await redisService.SetValue(refreshKey, refreshToken, TimeSpan.FromDays(jwtService.getRefreshTokenValidity()));
            await redisService.SetValue(accessKey, jwtToken, TimeSpan.FromMinutes(jwtService.getAccessTokenValidity()));
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(new Error("Redis.SaveFailed", $"Failed to save tokens: {ex.Message}"));
        }
        var redirectUrl = $"https://booking-space-kappa.vercel.app/home" +
                          $"?accessToken={jwtToken}" +
                          $"&refreshToken={refreshToken}" +
                          $"&email={user.Email}" +
                          $"&fullName={user.FullName}" +
                          $"&avatarUrl={user.AvatarUrl}" +
                          $"&phoneNumber={user.PhoneNumber}" +
                          $"&id={user.Id}" +
                          $"&userName={user.UserName}";
       return Result<string>.Success(redirectUrl); 

    }
}