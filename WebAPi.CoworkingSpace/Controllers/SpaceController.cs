using Application.DTOs;
using Application.PriceService.DTOs;
using Application.SpaceService.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[Route("[controller]")]
[ApiController]
public class SpaceController(IMediator mediator): Controller
{
    [HttpPost("CreateSpace")]
    public async Task<IActionResult> SetUpDailySpacePrice([FromForm] CreateSpaceCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}