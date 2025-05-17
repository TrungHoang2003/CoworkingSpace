using Application.UserService.CQRS.Commands;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator):Controller
{
    [HttpPost("UpdateAvatar")]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateUserAvatarCommand request)
    {
        var result = await mediator.Send(request);
        if (result.IsSuccess) return Ok(result);
        return BadRequest(result.Error);
    }
}