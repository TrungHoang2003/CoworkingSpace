using CloudinaryDotNet.Actions;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Error = Domain.ResultPattern.Error;

namespace Application.SpaceService.CQRS.Commands;

public sealed record UpdateSpaceVirtualVideoCommand(IFormFile Video, int SpaceId):IRequest<Result>;

internal class UpdateSpaceVirtualVideoCommandHandler(CloudinaryService cloudinaryService, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSpaceVirtualVideoCommand, Result>
{
    public async Task<Result> Handle(UpdateSpaceVirtualVideoCommand request, CancellationToken cancellationToken)
    {
        // var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        // if (!validatorResult.IsValid)
        //     return Result.Failure(new Error("Validation Errors",
        //         string.Join("; ", validatorResult.Errors.Select(x => x.ErrorMessage).ToList())));

        var spaceVideo = await unitOfWork.SpaceAsset.GetByType(request.SpaceId, SpaceAssetType.VirtualVideo);
        if (spaceVideo == null)
        {
            var result = await cloudinaryService.UploadImage(request.Video);
            if (result is null) return CloudinaryErrors.UploadSpaceVirtualVideoFailed;
            
            var video = new SpaceAsset
            {
                SpaceId = request.SpaceId,
                Url = result,
                Type = SpaceAssetType.VirtualVideo,
            };
            await unitOfWork.SpaceAsset.Create(video);
        }else
        {
            var result = await cloudinaryService.UpdateImage(request.Video, spaceVideo.Url); 
            if (result is null) return CloudinaryErrors.UpdateSpaceVirtualVideoFailed;
            spaceVideo.Url = result;
        }
        return Result.Success();
    }
}