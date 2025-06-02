using Application.Services.Spaces.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPi.CoworkingSpace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuggestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuggestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetSuggestions([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Ok(new List<string>());
        }

        var command = new GetSuggestionsCommand(q);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}
