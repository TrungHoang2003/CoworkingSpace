using Domain.DTOs;
using Domain.Entites;
using Infrastructure.Common;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.VenueService.Commands;

public sealed record SignUpVenueCommand(SignUpVenueDTO SignUpVenueDto): IRequest<Result>;

public class SignUpVenueCommandHandler(CloudinaryService cloudinaryService, UserManager<User> userManager, IUnitOfWork unitOfWork) : IRequestHandler<SignUpVenueCommand, Result>
{
    public async Task<Result> Handle(SignUpVenueCommand command, CancellationToken cancellationToken)
    {
        var venueType = await unitOfWork.Venue.GetVenueTypeById(command.SignUpVenueDto.venueTypeId);

        if (venueType == null)
            return VenueErrors.VenueNotFound;

        if (command.SignUpVenueDto.userAvatar != null)
        {
            var userAvatarUrl = await cloudinaryService.UploadImage(command.SignUpVenueDto.userAvatar);

            if (userAvatarUrl == null)
                return CloudinaryErrors.UploadUserAvatarFailed; 
            
            command.SignUpVenueDto.UserAvatarUrl = userAvatarUrl;
        }

        var reuslt = unitOfWork.User.GetUserIdFromJwt();
        if (reuslt.IsFailure)
            return reuslt.Error;
        
        var user = await userManager.FindByIdAsync(reuslt.Value.ToString());
        
        if (user == null)
            return AuthenErrors.UserNotFound;

        user.PhoneNumber = command.SignUpVenueDto.phoneNumber;
        user.AvatarUrl = command.SignUpVenueDto.UserAvatarUrl; 
        
        var updatedResult = await userManager.UpdateAsync(user);
        if(!updatedResult.Succeeded)
            return Result.Failure( new Error("Update user failed", string.Join(",", updatedResult.Errors.Select(e=>e.Description).ToList())));

        if (command.SignUpVenueDto.VenueLogo!= null)
        {
            var venueLogoUrl = await cloudinaryService.UploadImage(command.SignUpVenueDto.VenueLogo);
            
            if (venueLogoUrl == null)
                return CloudinaryErrors.UploadVenueLogoFailed;
            
            command.SignUpVenueDto.VenueLogoUrl = venueLogoUrl;
        }
        
        var venue = new Venue
        {
            Name = command.SignUpVenueDto.VenueName,
            VenueTypeId = command.SignUpVenueDto.venueTypeId,
            VenueLogoUrl = command.SignUpVenueDto.VenueLogoUrl,
            HostId = user.Id
        };
        
        await unitOfWork.Venue.Create(venue);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}