
using Infrastructure.Common;

namespace Domain.Errors;

public sealed record CloudinaryErrors
{
    public static readonly Error UploadUserAvatarFailed = new("CloudinaryErros","Upload user avatar failed");
    public static readonly Error UploadVenueLogoFailed= new("CloudinaryErros","Upload venue image failed");
}