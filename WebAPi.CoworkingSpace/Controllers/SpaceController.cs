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
    public async Task<IActionResult> CreateSpace([FromForm] CreateSpaceCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("UpdateSpace")]
    public async Task<IActionResult> UpdateSpace([FromBody] UpdateSpaceCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("DeleteSpaceAsset")]
    public async Task<IActionResult> DeleteSpaceImage([FromBody] DeleteSpaceAssetCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("UpdateSpaceVideo")]
    public async Task<IActionResult> UpdateSpaceVideo([FromForm] UpdateSpaceVideoCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("UpdateVirtualSpaceVideo")]
    public async Task<IActionResult> UpdateVirtualSpaceVideo([FromForm] UpdateSpaceVirtualVideoCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("AddSpaceImage")]
    public async Task<IActionResult> AddSpaceImage([FromForm] AddSpaceImageCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
