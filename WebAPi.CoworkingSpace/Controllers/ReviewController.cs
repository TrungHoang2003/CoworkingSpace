using Application.Services.Reviews.CQRS.Commands;
using Application.Services.Reviews.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingSpace.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController(IMediator mediator):Controller
{
   [HttpPost("CreateReview")]
   public async Task<IActionResult> CreateReview([FromBody] CreateReview request)
   {
      var result = await mediator.Send(request);
      if (result.IsSuccess) return Ok(result);
      return BadRequest(result.Error);
   }
   
   [HttpGet("GetSpaceReviews/{spaceId}")]
   public async Task<IActionResult> GetSpaceReviews(int spaceId)
   {
      var result = await mediator.Send(new GetSpaceReviews(spaceId));
      if (result.IsSuccess) return Ok(result.Value);
      return BadRequest(result.Error);
   }
}