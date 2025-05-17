using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var acc = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]);

        _cloudinary = new Cloudinary(acc);
    }

    //=================== IMAGE ===================

    public async Task<string?> UploadImage(string base64Image)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription("base64_image", base64Image),
            Folder = "Coworking-Space/Images"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }

    public async Task<string?> UpdateImage(string base64Image, string existingUrl)
    {
        var publicId = ExtractPublicId(existingUrl);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription("base64_image", base64Image),
            PublicId = publicId
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }

    //=================== VIDEO ===================

    public async Task<string?> UploadVideo(string base64Video)
    {
        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription("base64_video", base64Video),
            Folder = "Coworking-Space/Videos"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }

    public async Task<string?> UpdateVideo(string base64Video, string existingUrl)
    {
        var publicId = ExtractPublicId(existingUrl);

        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription("base64_video", base64Video),
            PublicId = publicId
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }

    //=================== RAW FILE ===================

    public async Task<string?> UploadRaw(string base64File)
    {
        var uploadParams = new RawUploadParams
        {
            File = new FileDescription("base64_file", base64File),
            Folder = "Coworking-Space/Files"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl?.ToString();
    }

    //=================== DELETE ===================

    public async Task<bool> DeleteFile(string url)
    {
        var publicId = ExtractPublicId(url);
        var deletionParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);

        return result.Result == "ok";
    }

    //=================== HELPER ===================

    private string ExtractPublicId(string url)
    {
        var uri = new Uri(url);
        var path = uri.AbsolutePath;
        var uploadIndex = path.IndexOf("/Coworking-Space/") + "/Coworking-Space/".Length;
        var withoutVersion = path.Substring(uploadIndex);
        var withoutExtension = withoutVersion.Substring(0, withoutVersion.LastIndexOf('.'));

        return "Coworking-Space/" + withoutExtension;
    }
}
