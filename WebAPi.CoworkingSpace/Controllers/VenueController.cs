using Application.VenueService.Commands;
using Domain.DTOs;
using Domain.Entites;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]

public class VenueController(IVenueRepository repository, IMediator mediator): Controller
{
    [Authorize]
    [HttpGet("GetVenueTypes")]
    public async Task<IActionResult> GetVenueTypes()
    { 
        var result = await repository.GetVenueTypes();
        return Ok(result);
    }
    
    [HttpGet("GetVenueTypeById/{venueTypeId}")]
    public async Task<IActionResult> GetVenueTypeById(int venueTypeId)
    {
        var result = await repository.GetVenueTypeById(venueTypeId);
        return Ok(result);
    }
    
    [HttpPost("RegisterVenue")]
    public async Task<IActionResult> RegisterVenue([FromBody] SignUpVenueDTO signUpVenueDto)
    {
        var result = await mediator.Send(new SignUpVenueCommand(signUpVenueDto));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
}