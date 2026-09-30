using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SkillLoop.Application.Interfaces.IService;

namespace SkillLoop.Infrasturcture.Services
{
    public class CloudinaryFileStorageService : IFileStorageService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryFileStorageService(IConfiguration configuration)
        {
            var url = configuration["Cloudinary:Url"];
            if (string.IsNullOrWhiteSpace(url)) throw new InvalidOperationException("Cloudinary is not configured.");
            _cloudinary = new Cloudinary(url);
        }
        public async Task<string> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file.Length == 0) throw new InvalidOperationException("Image is empty.");
            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("Only image files are allowed.");
            if (file.Length > 5 * 1024 * 1024) throw new InvalidOperationException("Image must not exceed 5 MB.");
            await using var stream = file.OpenReadStream();
            var result = await _cloudinary.UploadAsync(new ImageUploadParams { File = new FileDescription(file.FileName, stream), Folder = "skillloop/users" });
            if (result.Error != null) throw new InvalidOperationException(result.Error.Message);
            return result.SecureUrl.ToString();
        }
    }
}
