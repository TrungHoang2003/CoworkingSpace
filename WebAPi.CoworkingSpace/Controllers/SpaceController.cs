using Application.DTOs;
using Application.SpaceService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[Route("[controller]")]
[ApiController]
public class SpaceController(IMediator mediator): Controller
{
    [HttpPost("SetUpHourlySpace")]
    public async Task<IActionResult> SetUpHourlySpace([FromForm] SetHourlySpaceRequest request)
    {
        var result = await mediator.Send(new SetHourlySpaceCommand(request));
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result.Error);
    }
}