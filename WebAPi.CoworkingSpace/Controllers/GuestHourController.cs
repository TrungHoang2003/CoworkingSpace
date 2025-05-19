using Application.Services.GuestHours.CQRS.Queries;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class GuestHourController(IMediator mediator): Controller
{
    [HttpGet("GetVenueGuestHours/{venueId}")]
    public async Task<IActionResult> GetGuestHours(int venueId)
    {
        var guestHours = await mediator.Send(new GetVenueGuestHoursQuery(venueId));
        if(!guestHours.IsSuccess)
            return BadRequest(guestHours.Error);
        return Ok(guestHours.Value);
    }
}