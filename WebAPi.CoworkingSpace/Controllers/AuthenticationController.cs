// API/Controllers/AuthenticationController.cs

using Application.AuthService.CQRS.Commands;
using Application.AuthService.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("GoogleLogin")]
    public async Task<IActionResult> GoogleLogin()
    {
        var query = new GetGoogleAuthUrlQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Redirect(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("GoogleCallBack")]
    public async Task<IActionResult> GoogleCallback([FromQuery] string code)
    {
        var command = new GoogleCallbackCommand(code);
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Redirect(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutCommand();
        var result = await _mediator.Send(command);
        
        if(!result.IsSuccess)
            return BadRequest(result.Error);
    
        return Ok(result.Value);
    }
}