using Application.HolidayService.Commands;
using Domain.DTOs;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class HolidayController(IMediator mediator) : Controller
{
    [HttpPut("SetObservedHolidayForVenue")]
    public async Task<IActionResult> SetObservedHolidayForVenue([FromBody] UpdateHolidayRequest request)
    {
        var result = await mediator.Send(new UpdateVenueHolidayCommand(request));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
}