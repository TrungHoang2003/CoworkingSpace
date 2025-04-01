using Domain.Entites;
using Domain.Responses;
using Infrastructure.Common;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Interfaces;

public interface IAuthenticationRepository
{
    Task<Result> Register( UserRegisterDTO userRegisterDto);
    Task<Result<LoginResponse>> Login(UserLoginDTO userLoginDto);
    Task<Result<string>> GoogleLogin();
    Task<Result<string>> GoogleCallBack(string code);
}