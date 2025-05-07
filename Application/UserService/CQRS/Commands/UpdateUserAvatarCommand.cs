using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UserService.CQRS.Commands;

public sealed record UpdateUserAvatarCommand(IFormFile Avatar, int userId) : IRequest<Result>;

public class UpdateUserAvatarCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService) : IRequestHandler<UpdateUserAvatarCommand, Result>
{
    public async Task<Result> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.GetById(request.userId);
        if (user == null) return AuthenErrors.UserNotFound;

        var url = await cloudinaryService.UploadImage(request.Avatar);
        if (url == null) return CloudinaryErrors.UploadUserAvatarFailed;

        user.AvatarUrl = url;
        await unitOfWork.User.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}