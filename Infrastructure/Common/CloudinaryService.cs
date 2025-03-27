using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Common;

public class CloudinaryService()
{
    private readonly Cloudinary cloudinary= null!;

    public CloudinaryService(IConfiguration configuration) : this()
    {
        var acc = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]);

        cloudinary = new Cloudinary(acc);
    }

    public async Task<string?> UploadImage(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "Coworking-Space"
        };
        
        var uploadResult = await cloudinary.UploadAsync(uploadParams);
        return uploadResult.SecureUrl.ToString();
    }
}