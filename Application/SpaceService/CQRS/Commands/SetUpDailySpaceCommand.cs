using Application.PriceService.DTOs;
using Application.SpaceService.DTOs;
using Application.SpaceService.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

namespace Application.SpaceService.CQRS.Commands;

public sealed record SetUpDailySpaceCommand(
    int VenueId,
    int? SpaceId,
    SpaceInfos? SpaceInfos,
    List<SpaceImagesDto>? SpaceImages,
    DailySpacePriceDto? SpacePrice,
    List<SpaceAmenityDto>? Amenities
) : IRequest<Result>;

public class SetUpDailySpaceCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService)
    : IRequestHandler<SetUpDailySpaceCommand, Result>
{
    public async Task<Result> Handle(SetUpDailySpaceCommand request, CancellationToken cancellationToken)
    {
        var venueExists = await unitOfWork.Venue.GetById(request.VenueId);
        if (venueExists is null) return VenueErrors.VenueNotFound;

        Space space;
        
        if(request.SpaceId != null)
        {
            var spaceExists = await unitOfWork.Space.GetById(request.SpaceId.Value);
            if (spaceExists is null) return SpaceErrors.SpaceNotFound;
            space = spaceExists; 
            
        }
        else
        {
            space = new Space
            {
                VenueId = request.VenueId
            };
        }
        
        if (request.SpaceInfos != null)
        {
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

            space = spaceInfos.ToSpace(space, videoUrl, virtualVideoUrl, pdfFlyerUrl);
            await unitOfWork.Space.Update(space);
        }

        if (request.SpaceImages is { Count: > 0 })
        {
            var spaceImagesDto = request.SpaceImages;
            foreach (var image in spaceImagesDto)
            {
                if (image.isCreate)
                {
                    var newImage = image.ToSpaceImage();
                    newImage.SpaceId = request.SpaceId.Value;
                    await unitOfWork.SpaceImage.Create(newImage);
                }
                else
                {
                    var existingImage = await unitOfWork.SpaceImage.GetById(image.ImageId!.Value);
                    if (existingImage is null) return SpaceErrors.SpaceImageNotFound;
                    existingImage.Type = image.Type;
                }
            }
        }

        if (request.SpacePrice != null)
        {
            var spacePriceDto = request.SpacePrice;
            if(spacePriceDto.IsFree)
                spacePriceDto.Amount = 0;
            space.Price = new Price
            {
                Amount = spacePriceDto.Amount,
            }; 
            await unitOfWork.Space.Update(space);
        }

        if (request.Amenities!= null)
        {
            foreach (var amenityDto in request.Amenities)
            {
                var amenityExists = await unitOfWork.Amenity.FindById(amenityDto.AmenityId);
                if(!amenityExists) return AmenityErrors.AmenityNotFound;
                    
                if (amenityDto.IsRemove)
                {
                    var existingSpaceAmenity = await unitOfWork.SpaceAmenity.Get(amenityDto.AmenityId, space.SpaceId);
                    if(existingSpaceAmenity is null) return AmenityErrors.SpaceAmenityNotFound;
                    await unitOfWork.SpaceAmenity.Delete(existingSpaceAmenity);
                }
                  
                var newSpaceAmenity =  new SpaceAmenity
                {
                    SpaceId = space.SpaceId,
                    AmenityId = amenityDto.AmenityId
                };
                await unitOfWork.SpaceAmenity.Create(newSpaceAmenity);
            }
        }
        return Result.Success();
    }
}