using Application.Services.Bookings.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController(IMediator mediator): Controller
{
   [HttpPost("BookDailySpace")]
   public async Task<IActionResult> BookDailySpace([FromBody] DailySpaceBookCommand command)
   {
       var result = await mediator.Send(command);
       if (result.IsFailure)
           return BadRequest(result.Error);
       return Ok(result.Value);
   }
   
    [HttpPost("CreatePayment")]
    public async Task<IActionResult> CreatePayment([FromBody] int reservationId)
    {
        var result = await mediator.Send(new CreatePaymentCommand(reservationId));
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }
}
   