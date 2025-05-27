using Application.SharedServices;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Error = Domain.ResultPattern.Error;

namespace Application.SpaceService.CQRS.Commands;

public sealed record UpdateSpaceVirtualVideo(string Video , int SpaceId):IRequest<Result>;

internal class UpdateSpaceVirtualVideoCommandHandler(CloudinaryService cloudinaryService, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSpaceVirtualVideo, Result>
{
    public async Task<Result> Handle(UpdateSpaceVirtualVideo request, CancellationToken cancellationToken)
    {
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