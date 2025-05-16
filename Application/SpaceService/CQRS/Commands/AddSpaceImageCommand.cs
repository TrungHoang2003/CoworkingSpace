using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using FluentValidation;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.SpaceService.CQRS.Commands;

public sealed record AddSpaceImageCommand(IFormFile Image, int SpaceId, SpaceAssetType Type):IRequest<Result>;

public class AddSpaceImageCommandHandler(IUnitOfWork unitOfWork, IValidator<AddSpaceImageCommand> validator, CloudinaryService cloudinaryService)
    : IRequestHandler<AddSpaceImageCommand, Result>
{
    public async Task<Result> Handle(AddSpaceImageCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
            return Result.Failure(new Error("Validation Errors",
                string.Join("; ", validatorResult.Errors.Select(x => x.ErrorMessage).ToList())));
        
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