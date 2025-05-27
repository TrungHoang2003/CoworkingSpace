using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Reviews.CQRS.Commands;

public sealed record CreateReview(
    int SpaceId, 
    int Rating,
    string? Comment
    ):IRequest<Result>;

public class CreateReviewCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateReview, Result>
{
    public async Task<Result> Handle(CreateReview request, CancellationToken cancellationToken)
    {
        // Lấy userId từ JWT
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        // Tìm kiếm người dùng trong db
        var user = await userManager.FindByIdAsync(userId!);
        if (user == null) return AuthenErrors.NotLoggedIn;
        
        var space = await unitOfWork.Space.FindById(request.SpaceId);
        if(!space) return SpaceErrors.SpaceNotFound;

        var review = new Review
        {
            SpaceId = request.SpaceId,
            CustomerId= user.Id,
            Rating = request.Rating,
            Comment = request.Comment
        };
        await unitOfWork.Review.Create(review);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

}