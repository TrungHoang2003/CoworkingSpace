using System.Text.Json;
using Domain.Entites;
using Domain.Entities;
using Domain.Responses;
using Google.Apis.Auth;
using Infrastructure.Common;
using Infrastructure.DTOs;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories;

public class AuthenticationRepository(IConfiguration configuration, UserManager<User> userManager,
    RoleManager<Role> roleManager, JwtService jwtService, RedisService redis): IAuthenticationRepository
{
    public async Task<Result> GoogleRegister(UserRegisterDTO userRegisterDto)
    {
        var user = new User
        {
            UserName = userRegisterDto.UserName,
            FullName = userRegisterDto.FullName,
            Email = userRegisterDto.Email,
            AvatarUrl = userRegisterDto.AvatarUrl,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(user); 

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e=>e.Description).ToList();
            return Result.Failure(new Error("Register failed", string.Join(",",errors)));
        }    
        
        var hostRole = await roleManager.RoleExistsAsync("Customer");
        if(!hostRole) await roleManager.CreateAsync(new Role("Customer"));
        
        var addRoleResult = await userManager.AddToRoleAsync(user, "Customer");
        if (!addRoleResult.Succeeded)
            return Result.Failure(new Error("Add role failed", string.Join(",", addRoleResult.Errors.Select(e => e.Description))));

        return Result.Success();
    }
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
        
        var customerRole= await roleManager.RoleExistsAsync("Customer");
        if(!customerRole) await roleManager.CreateAsync(new Role("Customer"));
        
        var addRoleResult = await userManager.AddToRoleAsync(user, "Customer");
        if (!addRoleResult.Succeeded)
            return Result.Failure(new Error("Add role failed", string.Join(",", addRoleResult.Errors.Select(e => e.Description))));

        return Result.Success();
    }

    public async Task<Result<LoginResponse>> Login(UserLoginDTO userLoginDto)
    {
        var user = await userManager.FindByNameAsync(userLoginDto.UserName!);
        
        if(user == null) return Result<LoginResponse>.Failure(AuthenErrors.UserNotFound);
        
        var result = await userManager.CheckPasswordAsync(user, userLoginDto.Password!);
        
        if(!result) return Result<LoginResponse>.Failure(AuthenErrors.WrongPassword);
        
        var roles = await userManager.GetRolesAsync(user);
        
        var accessToken = jwtService.GenerateJwtToken(user, "");
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

    public Task<Result<string>> GoogleLogin()
    {
        var clientId = configuration["Authentication:Google:ClientId"];
        
        if(clientId == null)
            return Task.FromResult(Result<string>.Failure(ConfigurationErrors.ClientIdNotFound));
        
        var redirectUri = "http://localhost:5196/Authentication/GoogleCallback";

        var scope = "openid profile email";
        var state = Guid.NewGuid().ToString();
        
        var url = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                  $"client_id={clientId}&" +
                  $"redirect_uri={redirectUri}&" +
                  $"response_type=code&" +
                  $"scope={scope}&" +
                  $"access_type=offline&" +
                  $"state={state}";
        
        return Task.FromResult(Result<string>.Success(url));
    }

    public async Task<Result<string>> GoogleCallBack(string code)
    {
        var tokenUrl = "https://oauth2.googleapis.com/token";
        
        var clientId = configuration["Authentication:Google:ClientId"];
        if(clientId == null) return Result<string>.Failure(ConfigurationErrors.ClientIdNotFound);
        
        var clientSecret = configuration["Authentication:Google:ClientSecret"];
        if (clientSecret == null) return Result<string>.Failure(ConfigurationErrors.ClientSecretNotFound);

        var values = new Dictionary<string, string>()
        {
            { "code", code },
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "redirect_uri", "http://localhost:5196/Authentication/GoogleCallback" },
            { "grant_type", "authorization_code" }
        };

        var content = new FormUrlEncodedContent(values);
        var httpClient = new HttpClient();
        var response = await httpClient.PostAsync(tokenUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();
        
        var tokenData = JsonSerializer.Deserialize<GoogleTokenResponse>(responseString);
        
        var payload = await GoogleJsonWebSignature.ValidateAsync(tokenData!.id_token);
        
        var user = await userManager.FindByEmailAsync(payload.Email);

        if (user == null)
        {
            var userRegisterDto = new UserRegisterDTO
            { 
                UserName = payload.Email,
                Email = payload.Email,
                FullName = payload.Name,
                AvatarUrl = payload.Picture
            };
            var result = await GoogleRegister(userRegisterDto);
            if(!result.IsSuccess) return Result<string>.Failure(result.Error);
        }
        
        var jwtToken = jwtService.GenerateJwtToken(user!,"");

        return Result<string>.Success($"http://localhost:3000/google-auth-success?token={jwtToken}");
    }
}