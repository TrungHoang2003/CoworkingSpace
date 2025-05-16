using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UserService.CQRS.Commands;

public sealed record UpdateUserAvatarCommand(IFormFile Avatar, int UserId) : IRequest<Result>;

public class UpdateUserAvatarCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService) : IRequestHandler<UpdateUserAvatarCommand, Result>
{
    public async Task<Result> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.GetById(request.UserId);
        if (user == null) return AuthenErrors.UserNotFound;

        if(user.AvatarUrl == null) // Chưa có avatar
        {
            var result = await cloudinaryService.UploadImage(request.Avatar);
            if (result is null) return CloudinaryErrors.UploadUserAvatarFailed;
            
            user.AvatarUrl = result;
        }
        else // Đã có avatar
        {
            var url = await cloudinaryService.UpdateImage(request.Avatar, user.AvatarUrl);
            if (url is null) return CloudinaryErrors.UpdateUserAvatarFailed;
            user.AvatarUrl = url;
        }

        await unitOfWork.User.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}