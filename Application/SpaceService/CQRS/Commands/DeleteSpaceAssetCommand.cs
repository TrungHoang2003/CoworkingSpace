using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

namespace Application.SpaceService.CQRS.Commands;

public sealed record DeleteSpaceAssetCommand(int SpaceId, string Url):IRequest<Result>;

public class DeleteSpaceAssetCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService)
    : IRequestHandler<DeleteSpaceAssetCommand, Result>
{
    public async Task<Result> Handle(DeleteSpaceAssetCommand request, CancellationToken cancellationToken)
    {
        var space = await unitOfWork.Space.FindById(request.SpaceId);
        if (!space) return SpaceErrors.SpaceNotFound;

        var asset = await unitOfWork.SpaceAsset.GetByUrl(request.SpaceId, request.Url);
        if (asset is null) return SpaceErrors.SpaceAssetNotFound;
        
        await unitOfWork.SpaceAsset.Delete(asset);
        await cloudinaryService.DeleteFile(asset.Url);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}