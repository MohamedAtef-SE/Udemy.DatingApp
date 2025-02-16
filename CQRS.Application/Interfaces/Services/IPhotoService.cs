using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsycn(string publicId);
    }
}
