using Application.SharedServices;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Auths.CQRS.Commands;

public sealed record GoogleCallbackCommand(string Code) : IRequest<Result<string>>;

public class GoogleCallbackCommandHandler(
    GoogleAuthService googleAuthService,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
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

        return Result<string>.Success($"http://localhost:3000?token={jwtToken}");
    }
}