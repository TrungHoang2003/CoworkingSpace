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
}