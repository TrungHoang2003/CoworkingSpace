using Application.DTOs;
using Application.VenueService.Commands;
using Domain.DTOs;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]

public class VenueController(IVenueRepository repository, IMediator mediator): Controller
{
    [HttpGet("GetVenueTypes")]
    public async Task<IActionResult> GetVenueTypes()
    { 
        var result = await repository.GetVenueTypes();
        return Json(result);
    }
    
    [HttpGet("GetVenueTypeById/{venueTypeId}")]
    public async Task<IActionResult> GetVenueTypeById(int venueTypeId)
    {
        var result = await repository.GetVenueTypeById(venueTypeId);
        return Ok(result);
    }
    
    [HttpPost("SignUpVenue")]
    public async Task<IActionResult> SignUpVenue([FromForm] SignUpVenueRequest signUpVenueRequest)
    {
        var result = await mediator.Send(new SignUpVenueCommand(signUpVenueRequest));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }

    [HttpPost("SetUpVenue")]
    public async Task<IActionResult> SetUpVenue([FromForm] SetUpVenueRequest setupVenueRequest)
    {
        var result = await mediator.Send(new SetUpVenueCommand(setupVenueRequest));
        if(!result.IsSuccess)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
}