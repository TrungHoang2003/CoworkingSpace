using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController(IUserRepository repository):Controller
{ 
    [HttpPost("UpdateAvatar")]
   public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile file )
   {
       var result = await repository.UpdateAvatar(file);
       return result.IsSuccess ? Ok() : BadRequest(result.Error);
       
   }
}