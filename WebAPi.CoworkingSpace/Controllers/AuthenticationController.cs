// API/Controllers/AuthenticationController.cs

using Application.Services.Auths.CQRS.Commands;
using Application.Services.Auths.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController(IMediator mediator) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("GoogleLogin")]
    public async Task<IActionResult> GoogleLogin()
    {
        var query = new GetGoogleAuthUrlQuery();
        var result = await mediator.Send(query);
        return result.IsSuccess ? Redirect(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("GoogleCallBack")]
    public async Task<IActionResult> GoogleCallback([FromQuery] string code)
    {
        var command = new GoogleCallbackCommand(code);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Redirect(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutCommand();
        var result = await mediator.Send(command);
        
        if(!result.IsSuccess)
            return BadRequest(result.Error);
    
        return Ok(result.Value);
    }
}