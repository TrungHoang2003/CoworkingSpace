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
    List<int>? AmenityIds
) : IRequest<Result>;

public class SetUpDailySpaceCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService)
    : IRequestHandler<SetUpDailySpaceCommand, Result>
{
    public async Task<Result> Handle(SetUpDailySpaceCommand request, CancellationToken cancellationToken)
    {
        var venueExists = await unitOfWork.Venue.GetById(request.VenueId);
        if (venueExists is null) return VenueErrors.VenueNotFound;

        if (request.SpaceId is not null)
        {
            if (request.SpaceInfos is not null)
            {
                var spaceInfos = request.SpaceInfos;
                var existingSpace = await unitOfWork.Space.GetById(request.SpaceId.Value);
                if (existingSpace is null) return SpaceErrors.SpaceNotFound;
                
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
                
                existingSpace = spaceInfos.ToSpace(existingSpace, videoUrl, virtualVideoUrl, pdfFlyerUrl);
                await unitOfWork.Space.Update(existingSpace);
            }

            if (request.SpaceImages is not null)
            {
                var spaceImages = request.SpaceImages;
            }
        }
    }

}