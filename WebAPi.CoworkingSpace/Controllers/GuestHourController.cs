using Application.GuestHoursService.Commands;
using Application.GuestHoursService.DTOs;
using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class GuestHourController(IMediator mediator): Controller
{
   [HttpPut("UpdateGuestHours")]
   public async Task<IActionResult> UpdateGuestHours([FromBody] UpdateGuestHoursRequest updateGuestHoursRequest)
   {
       var result = await mediator.Send(new UpdateGuestHoursCommand(updateGuestHoursRequest));
       if (result.IsFailure)
           return BadRequest(result.Error);
       
       return Ok(result);
   }
}