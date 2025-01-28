using CloudinaryDotNet.Actions;

namespace API.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsycn(string publicId);
    }
}
