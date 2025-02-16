using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using CQRS.Application.Interfaces.Services;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Commands.Users.DeletePhoto
{
    public class DeletePhotoCommandHandler(IPhotoService _photoService,IUnitOfWork _unitOfWork,IHttpContextAccessor _httpContextAccessor) : IRequestHandler<DeletePhotoCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User.GetUserName();
            if (username is null) return Result<bool>.Failure("username not found");
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user is null) return Result<bool>.Failure("user not found during delete this photo");
            var photo = user.Photos.FirstOrDefault(p => p.Id == request.PhotoId);
            if (photo is null) return Result<bool>.Failure("photo not found");
            if (photo.PublicId is not null)
            {
                var result = await _photoService.DeletePhotoAsycn(photo.PublicId);
                if (result.Error is not null) return Result<bool>.Failure(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if (await _unitOfWork.CompleteAsync()) return Result<bool>.Success(true);

            return Result<bool>.Failure("something went wrong during delete photo from database.");
        }
    }
}
