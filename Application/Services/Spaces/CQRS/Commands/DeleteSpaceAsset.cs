using Application.SharedServices;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.SpaceService.CQRS.Commands;

public sealed record DeleteSpaceAsset(int SpaceId, string Url):IRequest<Result>;

public class DeleteSpaceAssetCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService)
    : IRequestHandler<DeleteSpaceAsset, Result>
{
    public async Task<Result> Handle(DeleteSpaceAsset request, CancellationToken cancellationToken)
    {
        var space = await unitOfWork.Space.FindById(request.SpaceId);
        if (!space) return SpaceErrors.SpaceNotFound;

        var asset = await unitOfWork.SpaceAsset.GetByUrl(request.SpaceId, request.Url);
        if (asset is null) return SpaceErrors.SpaceAssetNotFound;
        
        await unitOfWork.SpaceAsset.Delete(asset);
        var result = await cloudinaryService.DeleteFile(asset.Url);
        if (!result) return CloudinaryErrors.DeleteSpaceAssetFailed;
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}