using Application.DTOs;
using Application.VenueService.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class ExceptionController(IMediator mediator): Controller
{
}