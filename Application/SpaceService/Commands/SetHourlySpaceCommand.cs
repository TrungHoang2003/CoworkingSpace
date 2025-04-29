using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

namespace Application.SpaceService.Commands;

public sealed record SetHourlySpaceCommand(SetHourlySpaceRequest SetHourlySpaceRequest):IRequest<Result>;

public class AddHourlySpaceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, CloudinaryService cloudinaryService) : IRequestHandler<SetHourlySpaceCommand, Result>
{
    public async Task<Result> Handle(SetHourlySpaceCommand command, CancellationToken cancellationToken)
    {
        var request = command.SetHourlySpaceRequest;
        
        //Check if Venue and SpaceType exist
        var venueExist= await unitOfWork.Venue.FindById(request.VenueId);
        if (!venueExist) return VenueErrors.VenueNotFound;
        
        var spaceTypeExist = await unitOfWork.SpaceType.FindById(request.SpaceTypeId);
        if (!spaceTypeExist) return SpaceErrors.SpaceTypeNotFound;
        
        //Check Space exist and update information, if not exist create a new one
        var space = await unitOfWork.Space.GetByIdAndVenue(request.SpaceId, request.VenueId);
        if (space == null)
        {
            var newSpace = mapper.Map<Space>(request);
            newSpace.ListingType = ListingType.Hourly;
            await unitOfWork.Space.Create(newSpace);
        }
        else
        {
            space.ListingType = ListingType.Hourly;
            mapper.Map(request, space);
            await unitOfWork.Space.Update(space);
        }

        //Check if request has Images, if yes, check if image exist, create new SpaceImage
        if (request.Images is { Count: > 0 })
        {
            foreach (var image in request.Images)
            {
                var imageUrl = await cloudinaryService.UploadImage(image);
                var spaceImage = new SpaceImage
                {
                    ImageUrl = imageUrl,
                    SpaceId = request.SpaceId
                };
                await unitOfWork.SpaceImage.Create(spaceImage);
            }
        }
        
        //Check if request has AmenityIds, if yes, check if amenity exist, create new SpaceAmenity
        if (request.AmenityIds is { Count: > 0 })
        {
            foreach (var amenityId in request.AmenityIds)
            {
                var amenityExist = await unitOfWork.Amenity.FindById(amenityId);
                if (!amenityExist) return Result.Failure(new Error("Amenity Error", "Cannot find amenity with Id = " + amenityId +""));
                var spaceAmenity = new SpaceAmenity
                {
                    AmenityId = amenityId,
                    SpaceId = request.SpaceId
                };
                await unitOfWork.SpaceAmenity.Create(spaceAmenity);
            }
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}