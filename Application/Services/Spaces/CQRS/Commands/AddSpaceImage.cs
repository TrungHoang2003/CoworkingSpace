using Application.SharedServices;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Spaces.CQRS.Commands;

public sealed record AddSpaceImage(string Image, int SpaceId, SpaceAssetType Type):IRequest<Result>;

public class AddSpaceImageCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService)
    : IRequestHandler<AddSpaceImage, Result>
{
    public async Task<Result> Handle(AddSpaceImage request, CancellationToken cancellationToken)
    {
        var space = await unitOfWork.Space.GetById(request.SpaceId);
        if (space is null) return SpaceErrors.SpaceNotFound;
        
        var result = await cloudinaryService.UploadImage(request.Image);
        if(result is null) return CloudinaryErrors.UploadSpaceImageFailed;
        
        var spaceImage = new SpaceAsset
        {
            SpaceId = space.SpaceId,
            Url = result,
            Type = request.Type,
        };
        await unitOfWork.SpaceAsset.Create(spaceImage);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}