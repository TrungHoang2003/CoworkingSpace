using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Collections.CQRS.Commands;

public sealed record CreateCollection(
    string Name
    ): IRequest<Result>;

public class CreateCollectionCommandHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, UserManager<User> userManager) :
    IRequestHandler<CreateCollection, Result>
{
    public async Task<Result> Handle(CreateCollection request, CancellationToken cancellationToken)
    {
        // Lấy userId từ JWT
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        // Tìm kiếm người dùng trong db
        var user = await userManager.FindByIdAsync(userId!);
        if (user == null) return AuthenErrors.NotLoggedIn;
        
        var collection = new Collection
        {
            UserId = Convert.ToInt32(userId),
            Name = request.Name
        };
        await unitOfWork.Collection.Create(collection);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}