using Application.VenueService.CQRS.Commands;
using Application.VenueService.CQRS.Queries;
using Domain.Errors;
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
    public async Task<IActionResult> SignUpVenue([FromBody] SignUpVenueCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }

    [HttpPost("SetUpVenue")]
    public async Task<IActionResult> SetUpVenue([FromBody] SetUpVenueCommand command)
    {
        var result = await mediator.Send(command);
        if(!result.IsSuccess)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
    
    [HttpPost("UpdateLogo")]
    public async Task<IActionResult> UpdateVenueLogo([FromBody] UpdateVenueLogoCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }

    [HttpGet("GetUserVenues")]
    public async Task<IActionResult> GetUserVenues()
    {
        var result = await mediator.Send(new GetUserVenuesQuery());
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpGet("GetVenueById/{venueId}")]
    public async Task<IActionResult> GetVenueById(int venueId)
    {
        var result = await repository.GetById(venueId);
        if(result == null)
            return BadRequest(VenueErrors.VenueNotFound);
        return Ok(result);
    }
}