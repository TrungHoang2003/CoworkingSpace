using Domain.DTOs;
using Domain.Entites;
using Domain.Responses;
using Infrastructure.Common;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Interfaces;

public interface IAuthenticationRepository
{
    Task<Result> Register( UserRegisterRequest userRegisterRequest);
    Task<Result<LoginResponse>> Login(UserLoginRequest userLoginRequest);
    Task<Result<string>> GoogleLogin();
    Task<Result<string>> GoogleCallBack(string code);
}