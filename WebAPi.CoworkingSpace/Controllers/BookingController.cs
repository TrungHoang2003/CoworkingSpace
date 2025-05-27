using Application.Services.Bookings.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController(IMediator mediator): Controller
{
   [HttpPost("BookDailySpace")]
   public async Task<IActionResult> BookDailySpace([FromBody] Book command)
   {
       var result = await mediator.Send(command);
       if (result.IsFailure)
           return BadRequest(result.Error);
       return Ok(result.Value);
   }
}
   