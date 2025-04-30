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
   [HttpPost("SetUpBookingWindow")]
   public async Task<IActionResult> SetUpBookingWindow([FromBody] BookingWindowDto dto)
   {
      var result = await mediator.Send(new SetBookingWindowCommand(dto));
      
      if (!result.IsSuccess)
         return Ok(result.Error);

      return Ok(result);
   }
}