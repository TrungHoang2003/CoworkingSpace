using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.VenueService.CQRS.Commands;

public sealed record UpdateVenueLogoCommand(string Logo, int VenueId) : IRequest<Result>;

public class UpdateVenueLogoCommandHandler(IUnitOfWork unitOfWork, CloudinaryService cloudinaryService) : IRequestHandler<UpdateVenueLogoCommand, Result>
{
    public async Task<Result> Handle(UpdateVenueLogoCommand request, CancellationToken cancellationToken)
    {
        var venue = await unitOfWork.Venue.GetById(request.VenueId);
        if (venue == null) return VenueErrors.VenueNotFound;

        if (venue.LogoUrl == null)
        {
            var url = await cloudinaryService.UploadImage(request.Logo);
            if (url == null) return CloudinaryErrors.UploadVenueLogoFailed;
            venue.LogoUrl = url;
        }
        else
        {
            var url = await cloudinaryService.UpdateImage(request.Logo, venue.LogoUrl);
            if (url == null) return CloudinaryErrors.UploadVenueLogoFailed;
            venue.LogoUrl = url;
        }
        await unitOfWork.Venue.Update(venue);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}