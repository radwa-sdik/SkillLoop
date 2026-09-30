using Microsoft.AspNetCore.Http;

namespace SkillLoop.Application.Interfaces.IService
{
    public interface IFileStorageService
    {
        Task<string> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default);
    }
}
