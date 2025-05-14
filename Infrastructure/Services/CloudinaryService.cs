using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
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

    public async Task<string?> UploadImage(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "Coworking-Space/Images"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        return result.SecureUrl?.ToString();
    }

    public async Task<string?> UpdateImage(IFormFile file, string existingUrl)
    {
        await using var stream = file.OpenReadStream();

        var publicId = ExtractPublicId(existingUrl);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            PublicId = publicId
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        return result.SecureUrl?.ToString();
    }

    public async Task<string?> UploadVideo(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "Coworking-Space/Videos"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        return result.SecureUrl?.ToString();
    }

    public async Task<string?> UpdateVideo(IFormFile file, string existingUrl)
    {
        await using var stream = file.OpenReadStream();

        var publicId = ExtractPublicId(existingUrl);

        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            PublicId = publicId
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        return result.SecureUrl?.ToString();
    }

    public async Task<string?> UploadRaw(IFormFile file)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new RawUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "Coworking-Space/Files"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        return result.SecureUrl?.ToString();
    }

    public async Task<bool> DeleteFile(string url)
    {
        var publicId = ExtractPublicId(url);

        var deletionParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deletionParams);

        return result.Result == "ok";
    }

    private string ExtractPublicId(string url)
    {
        var uri = new Uri(url);
        var path = uri.AbsolutePath; // /demo/image/upload/v17199999/Coworking-Space/Images/logo_abcd1234.png
        var uploadIndex = path.IndexOf("/upload/") + "/upload/".Length;
        var withoutVersion = path.Substring(uploadIndex);
        var withoutExtension = withoutVersion.Substring(0, withoutVersion.LastIndexOf('.'));

        return withoutExtension;
    }
}
