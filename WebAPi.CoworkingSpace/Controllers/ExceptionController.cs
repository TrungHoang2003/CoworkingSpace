using Application.Services.Exceptions.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class ExceptionController(IMediator mediator): Controller
{
    [HttpPost("UpdateException")]
    public async Task<IActionResult> UpdateException([FromBody] UpdateExceptionCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
    
    [HttpPost("AddException")]
    public async Task<IActionResult> AddException([FromBody] AddExceptionCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
}