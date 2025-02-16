using CQRS.Application.Helpers;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Commands.Users.UpdateUserProfilePicture
{
    public class UpdateUserProfilePictureCommandHandler(IHttpContextAccessor _httpContextAccessor,IUnitOfWork _unitOfWork) : IRequestHandler<UpdateUserProfilePictureCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateUserProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User.GetUserName();
            if(username is null) return Result<bool>.Failure("username not found");
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user is null) return Result<bool>.Failure("user not found");

            var currentPhoto = user.Photos.FirstOrDefault(photo => photo.IsMain);
            var pickedPhoto = user.Photos.FirstOrDefault(photo => photo.Id == request.PhotoId);
            if (pickedPhoto is null) return Result<bool>.Failure("can not find this photo");

            if (currentPhoto is not null && currentPhoto.Id != pickedPhoto.Id)
            {
                currentPhoto.IsMain = false;
            }

            if (pickedPhoto.IsMain) return Result<bool>.Success(true);
            pickedPhoto.IsMain = true;

            if (await _unitOfWork.CompleteAsync()) return Result<bool>.Success(true);
            return Result<bool>.Failure("something went wrong during complete updating user profile picture into database");
        }
    }
}
