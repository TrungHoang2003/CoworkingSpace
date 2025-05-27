using Application.SharedServices;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Spaces.CQRS.Commands;

public sealed record UpdateSpaceVideo(string Video, int SpaceId):IRequest<Result>;

internal class UpdateSpaceVideoCommandHandler(CloudinaryService cloudinaryService, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSpaceVideo, Result>
{
    public async Task<Result> Handle(UpdateSpaceVideo request, CancellationToken cancellationToken)
    {
        var space = await unitOfWork.Space.FindById(request.SpaceId);
        if (!space) return SpaceErrors.SpaceNotFound;

        var spaceVideo = await unitOfWork.SpaceAsset.GetByType(request.SpaceId, SpaceAssetType.Video);
        if (spaceVideo == null)
        {
            var result = await cloudinaryService.UploadImage(request.Video);
            if (result is null) return CloudinaryErrors.UploadSpaceVideoFailed;
            
            var video = new SpaceAsset
            {
                SpaceId = request.SpaceId,
                Url = result,
                Type = SpaceAssetType.Video,
            };
            await unitOfWork.SpaceAsset.Create(video);
        }else
        {
           var result = await cloudinaryService.UpdateImage(request.Video, spaceVideo.Url); 
            if (result is null) return CloudinaryErrors.UpdateSpaceVideoFailed;
           spaceVideo.Url = result;
        }
        return Result.Success();
    }
}