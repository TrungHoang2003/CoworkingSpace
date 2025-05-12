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
    SpaceInfos SpaceInfos,
    List<SpaceImagesDto>? SpaceImages,
    SpacePriceDto SpacePrice,
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

        var spaceType = await unitOfWork.SpaceType.GetById(request.SpaceInfos.SpaceTypeId);
        if (spaceType is null) return SpaceErrors.SpaceTypeNotFound;

        switch (request.SpaceInfos.ListingType)
        {
            case ListingType.Daily when !spaceType.IsDaiLySpaceType:
                return SpaceErrors.NotDailySpaceType;
            case ListingType.Monthly when spaceType.IsDaiLySpaceType:
                return SpaceErrors.NotMonthlySpaceType;
        }

        var spaceInfos = request.SpaceInfos;
        string? videoUrl = null;
        string? virtualVideoUrl = null;
        string? pdfFlyerUrl = null;
        if (spaceInfos.VirtualVideo != null)
        {
            virtualVideoUrl = await cloudinaryService.UploadImage(spaceInfos.VirtualVideo);
        }

        if (spaceInfos.Video != null)
        {
            videoUrl = await cloudinaryService.UploadImage(spaceInfos.Video);
        }

        if (spaceInfos.PdfFlyer != null)
        {
            pdfFlyerUrl = await cloudinaryService.UploadImage(spaceInfos.PdfFlyer);
        }

        var space = spaceInfos.ToSpace(videoUrl, virtualVideoUrl, pdfFlyerUrl);
        space.VenueId = request.VenueId;

        if (request.SpaceImages is { Count: > 0 })
        {
            var spaceImagesDto = request.SpaceImages;
            foreach (var imageDto in spaceImagesDto)
            {
                var imageUrl = await cloudinaryService.UploadImage(imageDto.Image!);
                var newSpaceImage = new SpaceImage
                {
                    SpaceId = space.SpaceId,
                    ImageUrl = imageUrl,
                    Type = imageDto.Type
                };
                space.SpaceImages ??= new List<SpaceImage>(); // Khởi tạo nếu null
                space.SpaceImages.Add(newSpaceImage);
            }
        }

        var price = new Price();
        switch (request.SpaceInfos!.ListingType)
        {
            case ListingType.Daily:
                price.TimeUnit = TimeUnit.Day;
                break;
            case ListingType.Monthly:
                price.TimeUnit = TimeUnit.Month;
                price.DiscountPercentage = request.SpacePrice.DiscountPercentage;
                price.SetupFee = request.SpacePrice.SetupFee;
                break;
        }

        price.Amount = request.SpacePrice.Amount;
        space.Price = price;

        if (request.AmenityIds is { Count: > 0 })
        {
            List<SpaceAmenity> listSpaceAmenity = [];
            foreach (var amenityId in request.AmenityIds)
            {
                var amenity = await unitOfWork.Amenity.GetById(amenityId);
                if (amenity == null)
                    return Result.Failure(new Error("Amenity Error",
                        "Amenity not found with Id = " + amenityId));

                var spaceAmenity = new SpaceAmenity { AmenityId = amenityId };
                listSpaceAmenity.Add(spaceAmenity);
            }
            space.Amenities = listSpaceAmenity;
        }

        await unitOfWork.Space.Create(space);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
    