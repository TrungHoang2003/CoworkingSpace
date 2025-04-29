using Application.ExceptionService.DTOs;
using Application.VenueService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class ExceptionController(IMediator mediator): Controller
{
    [HttpPost("SetUpExceptionRule")]
    public async Task<IActionResult> SetUpExceptionRule(SetUpExceptionRequest request)
    {
       var result =  await mediator.Send(new SetExceptionCommand(request));
       if (!result.IsSuccess)
           return BadRequest(result.Error);

       return Ok(result.IsSuccess);
    }
}