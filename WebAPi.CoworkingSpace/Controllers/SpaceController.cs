using Application.Services.Spaces.CQRS.Commands;
using Application.Services.Spaces.CQRS.Queries;
using Application.SpaceService.CQRS.Commands;
using Domain.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[Route("[controller]")]
[ApiController]
public class SpaceController(IMediator mediator) : Controller
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

    [HttpPost("GetSpaces")]
    public async Task<IActionResult> GetSpaces([FromBody] SpaceFilter spaceFilter)
    {
        var command = new GetSpacesCommand(spaceFilter);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("GetSuggestions")]
    public async Task<IActionResult> GetSuggestions([FromBody] GetSuggestionsCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Keyword))
        {
            return Ok(new List<string>());
        }
        var result = await mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}
