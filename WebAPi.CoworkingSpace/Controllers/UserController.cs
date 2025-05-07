using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository repository):Controller
{ 
}