using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository repository):Controller
{ 
    [HttpPost("UpdateAvatar")]
   public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile file )
   {
       var result = await repository.UpdateAvatar(file);
       return result.IsSuccess ? Ok() : BadRequest(result.Error);
   }
}