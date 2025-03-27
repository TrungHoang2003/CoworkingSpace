using Domain.Entites;
using Domain.Responses;
using Infrastructure.Common;
using Infrastructure.DTOs;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class AuthenticationRepository(UserManager<User> userManager, JwtService jwtService, RedisService redis): IAuthenticationRepository
{
    public async Task<Result> Register(UserRegisterDTO userRegisterDto)
    {
        var user = new User
        {
            UserName = userRegisterDto.UserName,
            FullName = userRegisterDto.FullName,
            Email = userRegisterDto.Email,
        };
        
        var result = await userManager.CreateAsync(user, userRegisterDto.Password!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e=>e.Description).ToList();
            return Result.Failure(new Error("Register failed", string.Join(",",errors)));
        }    
        
        var addRoleResult = await userManager.AddToRoleAsync(user, "Customer");
        
        if (!addRoleResult.Succeeded)
        {
            return Result.Failure(new Error("Add role failed", string.Join(",", addRoleResult.Errors.Select(e => e.Description))));
        }

        return Result.Success();
    }

    public async Task<Result<LoginResponse>> Login(UserLoginDTO userLoginDto)
    {
        var user = await userManager.FindByNameAsync(userLoginDto.UserName!);
        
        if(user == null) return Result<LoginResponse>.Failure(AuthenErrors.UserNotFound);
        
        var result = await userManager.CheckPasswordAsync(user, userLoginDto.Password!);
        
        if(!result) return Result<LoginResponse>.Failure(AuthenErrors.WrongPassword);
        
        var roles = await userManager.GetRolesAsync(user);
        
        var accessToken = jwtService.GenerateJwtToken(user, roles[0]);
        var refreshToken = jwtService.GenerateRefreshToken();
        var refreshKey = $"refreshToken:{user.Id}";
        var accessKey = $"accessToken:{user.Id}";

        try
        {
            await redis.SetValue(refreshKey, refreshToken,
                TimeSpan.FromDays(jwtService.getRefreshTokenValidity()));
            await redis.SetValue(accessKey, accessToken,
                TimeSpan.FromMinutes(jwtService.getAccessTokenValidity()));
        }
        catch (Exception e)
        {
            throw new Exception("Error when saving token to redis", e);
        }
        
        var tokens = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
        
        return Result<LoginResponse>.Success(tokens);
    }
}