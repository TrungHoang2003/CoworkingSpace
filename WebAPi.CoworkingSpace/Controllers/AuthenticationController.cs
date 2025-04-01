using System.Net;
using Infrastructure.Common;
using Infrastructure.DTOs;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(IAuthenticationRepository repository): Controller
{
   [HttpPost("Register")]
   public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDto)
   {
      var response = await repository.Register(userRegisterDto);
      
      if (!response.IsSuccess)
         return BadRequest(response.Error);
      
      return Ok("Register successfully");
   }
   
   [HttpPost("Login")]
   public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDto)
   {
      var response = await repository.Login(userLoginDto);
      
      if (!response.IsSuccess)
         return BadRequest(response.Error);
      
      return Ok(response.Value);
   }

   [HttpGet("GoogleLogin")]
   public async Task<IActionResult> GoogleLogin()
   {
      var response= repository.GoogleLogin();
      
      if(response.Result.IsFailure) return BadRequest(response.Result.Error);
      
      var url = response.Result.Value;
      
      return Redirect(url);
   }

   [HttpGet("GoogleCallBack")]
   public async Task<IActionResult> GoogleCallBack([FromQuery] string code)
   {
      var result = await repository.GoogleCallBack(code);
      if(!result.IsSuccess)
         return BadRequest(result.Error);

      var url = result.Value;
      
      return Redirect(url);
   }
}