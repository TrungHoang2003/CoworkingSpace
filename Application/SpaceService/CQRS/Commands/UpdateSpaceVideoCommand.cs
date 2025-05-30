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

public sealed record UpdateSpaceVideoCommand(IFormFile Video, int SpaceId):IRequest<Result>;

internal class UpdateSpaceVideoCommandHandler(CloudinaryService cloudinaryService, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSpaceVideoCommand, Result>
{
    public async Task<Result> Handle(UpdateSpaceVideoCommand request, CancellationToken cancellationToken)
    {
        // var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        // if (!validatorResult.IsValid)
        //     return Result.Failure(new Error("Validation Errors",
        //         string.Join("; ", validatorResult.Errors.Select(x => x.ErrorMessage).ToList())));

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