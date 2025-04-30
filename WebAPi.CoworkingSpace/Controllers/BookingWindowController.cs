using Application.DTOs;
using Application.VenueService.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingWindowController(IMediator mediator): Controller
{
 
}