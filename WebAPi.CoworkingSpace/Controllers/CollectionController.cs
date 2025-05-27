using Application.Services.Collections.CQRS.Commands;
using Application.Services.Collections.CQRS.Queries;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]

public class CollectionController(IMediator mediator):Controller
{
    [HttpPost("CreateCollection")]
    public async Task<IActionResult> CreateCollection([FromBody] CreateCollection request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }
    
    [HttpGet("GetSpaceCollections/{spaceId}")]
    public async Task<IActionResult> GetSpaceCollectionList(int spaceId)
    {
        var result = await mediator.Send(new GetSpaceCollections(spaceId));
        if(!result.IsSuccess)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }
    
    [HttpPost("AddSpaceToCollection")]
    public async Task<IActionResult> AddSpaceToCollection([FromBody] AddSpaceToCollection request)
    {
        var result = await mediator.Send(request);
        if(!result.IsSuccess)
            return BadRequest(result.Error);
        return Ok(result);
    }
    
    [HttpPost("RemoveSpaceFromCollection")]
    public async Task<IActionResult> RemoveSpaceFromCollection([FromBody] RemoveSpaceFromCollection request)
    {
        var result = await mediator.Send(request);
        if(!result.IsSuccess)
            return BadRequest(result.Error);
        return Ok(result);
    }
}