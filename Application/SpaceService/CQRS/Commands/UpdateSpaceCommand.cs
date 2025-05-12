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

public sealed record UpdateSpaceCommand(
    int SpaceId,
    SpaceInfos? SpaceInfos,
    SpacePriceDto? SpacePrice,
    List<int>? AmenityIds) : IRequest<Result>;

public class UpdateSpaceCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService, IValidator<UpdateSpaceCommand> validator)
    : IRequestHandler<UpdateSpaceCommand, Result>
{
    public async Task<Result> Handle(UpdateSpaceCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
            return Result.Failure(new Error("Validation Errors",
                string.Join("; ", validatorResult.Errors.Select(x => x.ErrorMessage).ToList())));
        
        var space = await unitOfWork.Space.GetById(request.SpaceId);
        if (space is null) return SpaceErrors.SpaceNotFound;

        if (request.SpaceInfos != null)
        {
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
            space = request.SpaceInfos.ToSpace(space, videoUrl, virtualVideoUrl, pdfFlyerUrl);
        }

        if (request.SpacePrice != null)
        {
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
        }

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

        await unitOfWork.Space.Update(space);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}