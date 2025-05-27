using Application.Services.Prices.DTOs;
using Application.Services.Spaces.DTOs;
using Application.Services.Spaces.Mappings;
using Application.SharedServices;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Spaces.CQRS.Commands;

public sealed record CreateSpace(
    int VenueId,
    SpaceInfoDto BasicInfo,
    SpaceAssetDto? Asset,
    List<SpaceImageDto>? Images,
    SpacePriceDto Price,
    List<int>? AmenityIds) : IRequest<Result>;

public class CreateSpaceCommandHandler(
    IUnitOfWork unitOfWork,
    CloudinaryService cloudinaryService)
    : IRequestHandler<CreateSpace, Result>
{
    public async Task<Result> Handle(CreateSpace request, CancellationToken cancellationToken)
    {
        var venueExists = await unitOfWork.Venue.GetById(request.VenueId);
        if (venueExists is null) return VenueErrors.VenueNotFound;

        var spaceType = await unitOfWork.SpaceType.GetById(request.BasicInfo.SpaceTypeId);
        if (spaceType is null) return SpaceErrors.SpaceTypeNotFound;
        
        if(request.BasicInfo.ListingType == ListingType.MonthOnly && spaceType.IsNormalSpaceType)
            return SpaceErrors.NotMonthOnlySpaceType;
        if(request.BasicInfo.ListingType == ListingType.Normal&& !spaceType.IsNormalSpaceType)
            return SpaceErrors.NotNormalSpaceType;
        
        var space = request.BasicInfo.ToSpace();
        space.VenueId = request.VenueId;

        if (request.Asset != null)
        {
            var spaceAsset = request.Asset;
            if (spaceAsset.VirtualVideo != null)
            {
                var result = await cloudinaryService.UploadImage(spaceAsset.VirtualVideo);
                if (result is null) return CloudinaryErrors.UploadSpaceVirtualVideoFailed;
                var asset = new SpaceAsset
                {
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
                var asset = new SpaceAsset
                {
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
                var asset = new SpaceAsset
                {
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

        var price = new Price
        {
            DiscountPercentage = request.Price.DiscountPercentage,
            SetupFee = request.Price.SetupFee,
            Amount = request.Price.Amount
        };

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
                (space.Amenities ??= new List<SpaceAmenity>()).Add(spaceAmenity);
            }
        }

        await unitOfWork.Space.Create(space);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
    