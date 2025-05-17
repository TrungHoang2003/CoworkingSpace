using System.Security.Claims;
using Application.VenueAddressService.DTOs;
using Application.VenueService.Mappings;
using Domain.Entites;
using Domain.Entities;
using Domain.Errors;
using Domain.ResultPattern;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.VenueService.CQRS.Commands;

public sealed record SignUpVenueCommand(
    string? UserAvatar,
    int VenueTypeId,
    string Name,
    string Description,
    string PhoneNumber,
    string? Logo,
    SignUpVenueAddressDto Address
) : IRequest<Result>;

public class SignUpVenueCommandHandler(
    CloudinaryService cloudinaryService,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork) : IRequestHandler<SignUpVenueCommand, Result>
{
    public async Task<Result> Handle(SignUpVenueCommand request, CancellationToken cancellationToken)
    {
        string? userAvatarUrl = null;
        string? venueLogoUrl= null;

        // Kiểm tra xem loại văn phòng có tồn tại không
        var venueType = await unitOfWork.Venue.GetVenueTypeById(request.VenueTypeId);
        if (venueType == null) return VenueErrors.VenueTypeNotFound;

        // Kiểm tra xem người dùng có upload avatar không, nếu có thì gọi API cloudinary và lưu Url vào db
        if (request.UserAvatar != null)
        {
            userAvatarUrl = await cloudinaryService.UploadImage(request.UserAvatar);
            if (userAvatarUrl == null) return CloudinaryErrors.UploadUserAvatarFailed;
        }

        // Lấy userId từ JWT
        var userId = httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        // Tìm kiếm người dùng trong db
        var user = await userManager.FindByIdAsync(userId!);
        if (user == null) return AuthenErrors.NotLoggedIn;

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
        user.PhoneNumber = request.PhoneNumber;
        user.AvatarUrl = userAvatarUrl;

        var updatedResult = await userManager.UpdateAsync(user);
        if (!updatedResult.Succeeded)
            return Result.Failure(new Error("Update user failed",
                string.Join(",", updatedResult.Errors.Select(e => e.Description).ToList())));

        // Kiểm tra xem người dùng có upload logo cho venue không, nếu có thì gọi API cloudinary và lưu Url vào db
        if (request.Logo!= null)
        {
            venueLogoUrl = await cloudinaryService.UploadImage(request.Logo);
            if (venueLogoUrl == null)
                return CloudinaryErrors.UploadVenueLogoFailed;
        }

        // Tạo Venue mới
        var venue = request.ToVenue(venueLogoUrl);
        
        // Đăng ký Host cho Venue
        venue.HostId = user.Id;
        
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