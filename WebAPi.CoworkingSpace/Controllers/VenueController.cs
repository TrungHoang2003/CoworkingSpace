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
    public async Task<IActionResult> SignUpVenue([FromForm] SignUpVenueDTO signUpVenueDto)
    {
        var result = await mediator.Send(new SignUpVenueCommand(signUpVenueDto));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
    
    [HttpPost("UpdateVenueDetails")]
    public async Task<IActionResult> UpdateVenueDetails([FromBody] UpdateVenueDetailsDTO updateVenueDetailsDto)
    {
        var result = await mediator.Send(new UpdateVenueDetailsCommand(updateVenueDetailsDto));
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result);
    }
}