using Application.BookingWindowService.Commands;
using Application.BookingWindowService.DTOs;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingWindowController(IMediator mediator): Controller
{
   [HttpPost("SetUpBookingWindow")]
   public async Task<IActionResult> SetUpBookingWindow([FromBody] SetBookingWindowRequest request)
   {
      var result = await mediator.Send(new SetBookingWindowCommand(request));
      
      if (!result.IsSuccess)
         return Ok(result.Error);

      return Ok(result);
   }
}