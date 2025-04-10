using System.Runtime.CompilerServices;
using System.Security.Claims;
using Domain.Entites;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.DbHelper;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class UserRepository(CloudinaryService cloudinaryService, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext) : GenericRepository<User>(dbContext), IUserRepository
{
    public async Task<Result> UpdateAvatar(IFormFile file)
    {
        var userId = GetUserIdFromJwt().Value;

        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return Result.Failure(AuthenErrors.UserNotFound);

        var imageUrl = await cloudinaryService.UploadImage(file);

        if (imageUrl == null)
            return CloudinaryErrors.UploadUserAvatarFailed;
        
        user.AvatarUrl = imageUrl;
        
        var updatedResult = await userManager.UpdateAsync(user);
        if(!updatedResult.Succeeded)
            return Result.Failure( new Error("Update user failed", string.Join(",", updatedResult.Errors.Select(e=>e.Description).ToList())));

        return Result.Success();
    }

    public Result<int> GetUserIdFromJwt()
    {
        var stringUserId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (stringUserId == null)
            return Result<int>.Failure(AuthenErrors.UserNotFound);

        if(!int.TryParse(stringUserId, out var userId)) 
            Result<int>.Failure(ParseErrors.ParseError);

        return Result<int>.Success(userId);
    }
}