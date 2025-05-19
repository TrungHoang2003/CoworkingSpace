using Application.BookingWindowService.CQRS.Commands;
using Application.Services.BookingWindows.CQRS.Commands;
using Application.Services.BookingWindows.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingWindowController(IMediator mediator): Controller
{
    [HttpPost("AddBookingWindow")]
    public async Task<IActionResult> AddBookingWindow([FromBody] AddBookingWindowCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
    
    [HttpPost("UpdateBookingWindow")]
    public async Task<IActionResult> UpdateBookingWindow([FromBody] UpdateBookingWindowCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
    
    [HttpGet("GetBookingWindow/{venueId}")]
    public async Task<IActionResult> GetBookingWindow(int venueId)
    {
        var result = await mediator.Send(new GetBookingWindowViewQuery(venueId));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}