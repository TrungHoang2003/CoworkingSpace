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
}