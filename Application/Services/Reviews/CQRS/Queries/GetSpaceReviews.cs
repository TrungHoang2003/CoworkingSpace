using Domain.Entities;
using Domain.ResultPattern;
using Domain.ViewModel;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Reviews.CQRS.Queries;

public sealed record GetSpaceReviews(int SpaceId):IRequest<Result<List<ReviewViewModel>>>;

public class GetSpaceReviewsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetSpaceReviews, Result<List<ReviewViewModel>>>
{
    public async Task<Result<List<ReviewViewModel>>> Handle(GetSpaceReviews request, CancellationToken cancellationToken)
    {
        var reviews = await unitOfWork.Review.GetSpaceReviews(request.SpaceId);
        return Result<List<ReviewViewModel>>.Success(reviews);
    }
}