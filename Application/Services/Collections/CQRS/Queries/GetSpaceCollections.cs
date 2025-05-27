using Application.Services.Collections.DTOs;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Domain.ViewModel;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Collections.CQRS.Queries;

public sealed record GetSpaceCollections(int SpaceId): IRequest<Result<List<CollectionViewModel>>>;

public class GetUserCollectionsQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<GetSpaceCollections, Result<List<CollectionViewModel>>>
{
    public async Task<Result<List<CollectionViewModel>>> Handle(GetSpaceCollections request, CancellationToken cancellationToken)
    {
        // Lấy userId từ JWT
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        // Tìm kiếm người dùng trong db
        
        var user = await userManager.FindByIdAsync(userId!);
        if (user == null) return Result<List<CollectionViewModel>>.Failure(AuthenErrors.NotLoggedIn);

        var spaceCollections = await unitOfWork.Collection.GetUserSpaceCollectionList(Convert.ToInt32(userId), request.SpaceId);
        return Result<List<CollectionViewModel>>.Success(spaceCollections);
    }
}
