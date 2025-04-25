using Application.VenueService.DTOs;
using AutoMapper;
using Domain.Entites;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Errors;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.VenueService.Commands;

public sealed record SignUpVenueCommand(SignUpVenueRequest SignUpVenueRequest): IRequest<Result>;

public class SignUpVenueCommandHandler(
    CloudinaryService cloudinaryService,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<SignUpVenueCommand, Result>
{
    public async Task<Result> Handle(SignUpVenueCommand command, CancellationToken cancellationToken)
    {
        string? userAvatarUrl = null;
        string? venueLogoUrl;

        // Kiểm tra xem loại văn phòng có tồn tại không
        var venueType = await unitOfWork.Venue.GetVenueTypeById(command.SignUpVenueRequest.VenueTypeId);
        if (venueType == null) return VenueErrors.VenueTypeNotFound;

        // Kiểm tra xem người dùng có upload avatar không, nếu có thì gọi API cloudinary và lưu Url vào db
        if (command.SignUpVenueRequest.HostInformation.UserAvatar != null)
        {
            userAvatarUrl = await cloudinaryService.UploadImage(command.SignUpVenueRequest.HostInformation.UserAvatar);
            if (userAvatarUrl == null) return CloudinaryErrors.UploadUserAvatarFailed;
        }

        // Lấy userId từ JWT
        var result = unitOfWork.User.GetUserIdFromJwt();
        if (result.IsFailure) return result.Error;

        // Tìm kiếm người dùng trong db
        var user = await userManager.FindByIdAsync(result.Value.ToString());
        if (user == null) return AuthenErrors.UserNotFound;

        // Kiểm tra đã có role Host chưa, nếu chưa tạo role Host và gán cho User
        var hostRole = await roleManager.RoleExistsAsync("Host");
        if (!hostRole) await roleManager.CreateAsync(new Role("Host"));
        
        var isHost = await userManager.IsInRoleAsync(user, "Host");

        if (!isHost)
        {
            var addToRoleResult = await userManager.AddToRoleAsync(user, "Host");
            if (!addToRoleResult.Succeeded)
                return Result.Failure(new Error("Role assignment failed",
                    string.Join(",", addToRoleResult.Errors.Select(e => e.Description).ToList())));
        }

        // Cập nhật thông tin người dùng
        user.PhoneNumber = command.SignUpVenueRequest.HostInformation.PhoneNumber;
        user.AvatarUrl = userAvatarUrl;

        var updatedResult = await userManager.UpdateAsync(user);
        if (!updatedResult.Succeeded)
            return Result.Failure(new Error("Update user failed",
                string.Join(",", updatedResult.Errors.Select(e => e.Description).ToList())));

        // Kiểm tra xem người dùng có upload logo cho venue không, nếu có thì gọi API cloudinary và lưu Url vào db
        if (command.SignUpVenueRequest.Logo!= null)
        {
            venueLogoUrl = await cloudinaryService.UploadImage(command.SignUpVenueRequest.Logo);

            if (venueLogoUrl == null)
                return CloudinaryErrors.UploadVenueLogoFailed;
        }

        // Tạo Venue mới và lưu vào db
        var venue = mapper.Map<Venue>(command.SignUpVenueRequest);
        
        // Cập nhật địa chỉ đầy đủ cho Venue
        venue.Address.UpdateFullAddress();
        await unitOfWork.Venue.Create(venue);

        // Init GuestHour cho Venue
        var guestHours = unitOfWork.GuestHour.GenerateDefaultGuestHours(venue);
        await unitOfWork.GuestHour.AddRangeAsync(guestHours);

        // add Holiday cho Venue
        var holidays = await unitOfWork.Holiday.GetAllHolidays();
        await unitOfWork.VenueHoliday.GenerateDefaultHolidays(holidays, venue);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}