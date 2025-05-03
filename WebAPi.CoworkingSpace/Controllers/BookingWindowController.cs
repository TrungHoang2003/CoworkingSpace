using Application.BookingWindowService;
using Application.DTOs;
using Application.VenueService.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingWindowController(IMediator mediator): Controller
{
    [HttpPost("AddBookingWindow")]
    public async Task<IActionResult> AddBookingWindow([FromBody] AddBookingWindowRequest addBookingWindowRequest)
    {
        var result = await mediator.Send(new AddBookingWindowCommand(addBookingWindowRequest));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
    
    [HttpPost("UpdateBookingWindow")]
    public async Task<IActionResult> UpdateBookingWindow([FromBody] UpdateBookingWindowRequest updateBookingWindowRequest)
    {
        var result = await mediator.Send(new UpdateBookingWindowCommand(updateBookingWindowRequest));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
}