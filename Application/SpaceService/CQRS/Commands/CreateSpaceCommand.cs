using Application.PriceService.DTOs;
using Application.SpaceService.DTOs;
using Application.SpaceService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using FluentValidation;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

namespace Application.SpaceService.CQRS.Commands;

public sealed record CreateSpaceCommand(
    int VenueId,
    SpaceInfoDto BasicInfo,
    SpaceAssetDto? Asset,
    List<SpaceImageDto>? Images,
    SpacePriceDto Price,
    List<int>? AmenityIds) : IRequest<Result>;

public class CreateSpaceCommandHandler(
    IUnitOfWork unitOfWork,
    CloudinaryService cloudinaryService,
    IValidator<CreateSpaceCommand> validator)
    : IRequestHandler<CreateSpaceCommand, Result>
{
    public async Task<Result> Handle(CreateSpaceCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
            return Result.Failure(new Error("Validation Errors",
                string.Join("; ", validatorResult.Errors.Select(x => x.ErrorMessage).ToList())));

        var venueExists = await unitOfWork.Venue.GetById(request.VenueId);
        if (venueExists is null) return VenueErrors.VenueNotFound;

        var spaceType = await unitOfWork.SpaceType.GetById(request.BasicInfo.SpaceTypeId);
        if (spaceType is null) return SpaceErrors.SpaceTypeNotFound;

        switch (request.BasicInfo.ListingType)
        {
            case ListingType.Daily when !spaceType.IsDaiLySpaceType:
                return SpaceErrors.NotDailySpaceType;
            case ListingType.Monthly when spaceType.IsDaiLySpaceType:
                return SpaceErrors.NotMonthlySpaceType;
        }

        var space = request.BasicInfo.ToSpace();
        space.VenueId = request.VenueId;

        if (request.Asset != null)
        {
            var spaceAsset = request.Asset;
            if (spaceAsset.VirtualVideo != null)
            {
                var result = await cloudinaryService.UploadImage(spaceAsset.VirtualVideo);
                if (result is null) return CloudinaryErrors.UploadSpaceVirtualVideoFailed;
                var asset = new SpaceAsset{
                    SpaceId = space.SpaceId,
                    Url = result,
                    Type = SpaceAssetType.VirtualVideo,
                };
                (space.SpaceAssets ??= new List<SpaceAsset>()).Add(asset);
            }

            if (spaceAsset.Video != null)
            {
                var result = await cloudinaryService.UploadImage(spaceAsset.Video);
                if (result is null) return CloudinaryErrors.UploadSpaceVideoFailed;
                var asset = new SpaceAsset{
                    SpaceId = space.SpaceId,
                    Url = result,
                    Type = SpaceAssetType.Video,
                };
                (space.SpaceAssets ??= new List<SpaceAsset>()).Add(asset);
            }

            if (spaceAsset.PdfFlyer != null)
            {
                var result = await cloudinaryService.UploadImage(spaceAsset.PdfFlyer);
                if (result is null) return CloudinaryErrors.UploadSpacePdfFlyerFailed;
                var asset = new SpaceAsset{
                    SpaceId = space.SpaceId,
                    Url = result,
                    Type = SpaceAssetType.Pdf,
                };
                (space.SpaceAssets ??= new List<SpaceAsset>()).Add(asset);
            }
        }

        if (request.Images is { Count: > 0 })
        {
            foreach (var imageDto in request.Images)
            {
                var result = await cloudinaryService.UploadImage(imageDto.Image!);
                if (result is null) return CloudinaryErrors.UploadSpaceImageFailed;
                var newSpaceImage = new SpaceAsset
                {
                    SpaceId = space.SpaceId,
                    Url = result,
                    Type = imageDto.Type,
                };
                (space.SpaceAssets ??= new List<SpaceAsset>()).Add(newSpaceImage);
            }
        }

        var price = new Price();
        switch (request.BasicInfo!.ListingType)
        {
            case ListingType.Daily:
                price.TimeUnit = TimeUnit.Day;
                break;
            case ListingType.Monthly:
                price.TimeUnit = TimeUnit.Month;
                price.DiscountPercentage = request.Price.DiscountPercentage;
                price.SetupFee = request.Price.SetupFee;
                break;
        }

        price.Amount = request.Price.Amount;
        space.Price = price;

        if (request.AmenityIds is { Count: > 0 })
        {
            foreach (var amenityId in request.AmenityIds)
            {
                var amenity = await unitOfWork.Amenity.GetById(amenityId);
                if (amenity == null)
                    return Result.Failure(new Error("Amenity Error",
                        "Amenity not found with Id = " + amenityId));

                var spaceAmenity = new SpaceAmenity { AmenityId = amenityId };
                (space.Amenities??= new List<SpaceAmenity>()).Add(spaceAmenity);
            }
        }

        await unitOfWork.Space.Create(space);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
    