using Application.Services.Prices.DTOs;
using Application.Services.Spaces.DTOs;
using Application.Services.Spaces.Mappings;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Services.Spaces.CQRS.Commands;

public sealed record UpdateSpace(
    int SpaceId,
    SpaceInfoDto? BasicInfo,
    SpacePriceDto? Price,
    List<int>? AmenityIds) : IRequest<Result>;

public class UpdateSpaceCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSpace, Result>
{
    public async Task<Result> Handle(UpdateSpace request, CancellationToken cancellationToken)
    {
        var space = await unitOfWork.Space.GetById(request.SpaceId);
        if (space is null) return SpaceErrors.SpaceNotFound;

        if (request.BasicInfo != null)
        {
            if (request.BasicInfo.ListingType == ListingType.MonthOnly && space.SpaceType.IsNormalSpaceType)
                return SpaceErrors.NotMonthOnlySpaceType;
            if (request.BasicInfo.ListingType == ListingType.Normal && !space.SpaceType.IsNormalSpaceType)
                return SpaceErrors.NotNormalSpaceType;

            var spaceType = await unitOfWork.SpaceType.GetById(request.BasicInfo.SpaceTypeId);
            if (spaceType is null) return SpaceErrors.SpaceTypeNotFound;

            space = request.BasicInfo.ToSpace();
        }

        if (request.Price != null)
        {
            var price = new Price
            {
                DiscountPercentage = request.Price.DiscountPercentage,
                SetupFee = request.Price.SetupFee,
                Amount = request.Price.Amount
            };

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