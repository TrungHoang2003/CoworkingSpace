using Application.Services.Spaces.CQRS.Commands;
using Application.SpaceService.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[Route("[controller]")]
[ApiController]
public class SpaceController(IMediator mediator): Controller
{
    [HttpPost("CreateSpace")]
    public async Task<IActionResult> CreateSpace([FromBody] CreateSpace command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("UpdateSpace")]
    public async Task<IActionResult> UpdateSpace([FromBody] UpdateSpace command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("DeleteSpaceAsset")]
    public async Task<IActionResult> DeleteSpaceImage([FromBody] DeleteSpaceAsset command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("UpdateSpaceVideo")]
    public async Task<IActionResult> UpdateSpaceVideo([FromBody] UpdateSpaceVideo command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("UpdateVirtualSpaceVideo")]
    public async Task<IActionResult> UpdateVirtualSpaceVideo([FromBody] UpdateSpaceVirtualVideo command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("AddSpaceImage")]
    public async Task<IActionResult> AddSpaceImage([FromBody] AddSpaceImage command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
