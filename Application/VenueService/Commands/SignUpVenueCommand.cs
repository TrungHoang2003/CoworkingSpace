using Domain.DTOs;
using Domain.Entites;
using Infrastructure.Common;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.VenueService.Commands;

public sealed record SignUpVenueCommand(SignUpVenueDTO SignUpVenueDto): IRequest<Result>;

public class SignUpVenueCommandHandler(
    CloudinaryService cloudinaryService,
    UserManager<User> userManager,
    IUnitOfWork unitOfWork) : IRequestHandler<SignUpVenueCommand, Result>
{
    public async Task<Result> Handle(SignUpVenueCommand command, CancellationToken cancellationToken)
    {
        string? userAvatarUrl = null;
        string? venueLogoUrl = null;

        var venueType = await unitOfWork.Venue.GetVenueTypeById(command.SignUpVenueDto.VenueTypeId);

        if (venueType == null)
            return VenueErrors.VenueNotFound;

        if (command.SignUpVenueDto.UserAvatar != null)
        {
            userAvatarUrl = await cloudinaryService.UploadImage(command.SignUpVenueDto.UserAvatar);

            if (userAvatarUrl == null)
                return CloudinaryErrors.UploadUserAvatarFailed;
        }

        var result = unitOfWork.User.GetUserIdFromJwt();
        if (result.IsFailure)
            return result.Error;

        var user = await userManager.FindByIdAsync(result.Value.ToString());

        if (user == null)
            return AuthenErrors.UserNotFound;

        user.PhoneNumber = command.SignUpVenueDto.PhoneNumber;
        user.AvatarUrl = userAvatarUrl;

        var updatedResult = await userManager.UpdateAsync(user);
        if (!updatedResult.Succeeded)
            return Result.Failure(new Error("Update user failed",
                string.Join(",", updatedResult.Errors.Select(e => e.Description).ToList())));

        if (command.SignUpVenueDto.VenueLogo != null)
        {
            venueLogoUrl = await cloudinaryService.UploadImage(command.SignUpVenueDto.VenueLogo);

            if (venueLogoUrl == null)
                return CloudinaryErrors.UploadVenueLogoFailed;
        }

        var venue = new Venue
        {
            Name = command.SignUpVenueDto.VenueName,
            VenueTypeId = command.SignUpVenueDto.VenueTypeId,
            VenueLogoUrl = venueLogoUrl,
            Description = command.SignUpVenueDto.VenueDescription,
            HostId = user.Id,

            Address = new VenueAddress
            {
                City = command.SignUpVenueDto.VenueCity,
                District = command.SignUpVenueDto.VenueDistrict,
                Street = command.SignUpVenueDto.VenueStreet,
                Latitude = command.SignUpVenueDto.VenueLatitude,
                Longitude = command.SignUpVenueDto.VenueLongitude,
            }
        };

        venue.Address.FullAddress = $"{venue.Address.Street}, {venue.Address.District}, {venue.Address.City}";
        await unitOfWork.Venue.Create(venue);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}