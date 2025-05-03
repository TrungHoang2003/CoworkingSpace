using Application.DTOs;
using Application.ExceptionService;
using Application.VenueService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class ExceptionController(IMediator mediator): Controller
{
    [HttpPost("UpdateException")]
    public async Task<IActionResult> UpdateException([FromBody] UpdateExceptionRequest updateExceptionRequest)
    {
        var result = await mediator.Send(new UpdateExceptionCommand(updateExceptionRequest));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
    
    [HttpPost("AddException")]
    public async Task<IActionResult> AddException([FromBody] AddExceptionRequest addExceptionRequest)
    {
        var result = await mediator.Send(new AddExceptionCommand(addExceptionRequest));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
}