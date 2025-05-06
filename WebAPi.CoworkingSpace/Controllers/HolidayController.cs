using Application.DTOs;
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
}