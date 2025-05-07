// API/Controllers/AuthenticationController.cs

using Application.AuthService.CQRS.Commands;
using Application.AuthService.CQRS.Queries;
using Domain.DTOs;
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
}