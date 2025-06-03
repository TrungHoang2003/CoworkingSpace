using Domain;
using Domain.Entities;
using Domain.Errors;
using Domain.Filters;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Spaces.CQRS.Queries;

public sealed record GetSpacesCommand(SpaceFilter SpaceFilter) : IRequest<Result<List<SpaceViewHolder>>>;

public class GetSpacesCommandHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) : IRequestHandler<GetSpacesCommand, Result<List<SpaceViewHolder>>>
{
    public async Task<Result<List<SpaceViewHolder>>> Handle(GetSpacesCommand request, CancellationToken cancellationToken)
    {
        // Lấy userId từ JWT
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        // Tìm kiếm người dùng trong db
        var user = await userManager.FindByIdAsync(userId!);
        if (user == null) return Result<List<SpaceViewHolder>>.Failure(AuthenErrors.NotLoggedIn);
        var spaces = await unitOfWork.Space.GetSpaces(request.SpaceFilter, user.Id);
        return Result<List<SpaceViewHolder>>.Success(spaces);
    }
}