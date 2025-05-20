using Domain.Entities;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Auths.CQRS.Commands;

public sealed record GoogleRegisterCommand(
    string UserName,
    string FullName,
    string Email,
    string? AvatarUrl)
    : IRequest<Result<string>>;
    
public class GoogleRegisterCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    : IRequestHandler<GoogleRegisterCommand, Result>
{
    private const string CustomerRole = "Customer";

    public async Task<Result> Handle(GoogleRegisterCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = command.UserName,
            FullName = command.FullName,
            Email = command.Email,
            AvatarUrl = command.AvatarUrl,
            EmailConfirmed = true // Google users have verified emails
        };

        var result = await userRepository.CreateAsync(user); // No password
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result.Failure(new Error("Register.Failed", string.Join(",", errors)));
        }

        var roleExists = await roleRepository.RoleExistsAsync(CustomerRole);
        if (!roleExists)
        {
            var createRoleResult = await roleRepository.CreateAsync(new Role(CustomerRole));
            if (!createRoleResult.Succeeded)
                return Result.Failure(new Error("Role.CreateFailed", string.Join(",", createRoleResult.Errors.Select(e => e.Description))));
        }

        var addRoleResult = await userRepository.AddToRoleAsync(user, CustomerRole);
        if (!addRoleResult.Succeeded)
            return Result.Failure(new Error("Role.AddFailed", string.Join(",", addRoleResult.Errors.Select(e => e.Description))));

        return Result.Success();
    }
}