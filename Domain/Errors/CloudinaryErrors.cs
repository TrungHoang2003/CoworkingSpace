
using Domain.ResultPattern;

namespace Domain.Errors;

public sealed record CloudinaryErrors
{
    public static readonly Error UploadUserAvatarFailed = new("Cloudinary Erros","Upload user avatar failed");
    public static readonly Error UpdateUserAvatarFailed = new("Cloudinary Erros","Update user avatar failed");
    public static readonly Error UploadVenueLogoFailed= new("Cloudinary Erros","Upload venue logo failed");
    public static readonly Error UploadSpaceVideoFailed= new("Cloudinary Erros","Upload space video failed");
    public static readonly Error UpdateSpaceVideoFailed= new("Cloudinary Erros","Update space video failed");
    public static readonly Error UploadSpaceVirtualVideoFailed= new("Cloudinary Erros","Upload space virtual video failed");
    public static readonly Error UpdateSpaceVirtualVideoFailed= new("Cloudinary Erros","Update space virtual video failed");
    public static readonly Error UploadSpacePdfFlyerFailed= new("Cloudinary Erros","Upload space pdf flyer failed");
    public static readonly Error UploadSpaceImageFailed= new("Cloudinary Erros","Upload space image failed");
}