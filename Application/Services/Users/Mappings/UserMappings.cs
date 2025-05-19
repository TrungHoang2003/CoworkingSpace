using Domain.Entities;
using Domain.ViewModel;

namespace Application.UserService.Mappings;

public static class UserMappings
{
    public static UserViewModel ToUserViewModel(this User model)
    {
        return new UserViewModel
        {
            Id = model.Id,
            Email = model.Email,
            UserName = model.UserName,
            PhoneNumber = model.PhoneNumber,
            FullName = model.FullName,
            AvatarUrl = model.AvatarUrl
        };
    }
}