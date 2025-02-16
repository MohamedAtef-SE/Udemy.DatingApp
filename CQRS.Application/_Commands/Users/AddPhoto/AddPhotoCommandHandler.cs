using AutoMapper;
using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using CQRS.Application.Interfaces.Services;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Commands.Users.AddPhoto
{
    internal class AddPhotoCommandHandler(IPhotoService _photoService,IHttpContextAccessor _httpContextAccessor, IUnitOfWork _unitOfWork,IMapper _mapper) : IRequestHandler<AddPhotoCommand, Result<PhotoDTO>>
    {
        public async Task<Result<PhotoDTO>> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor.HttpContext?.User.GetUserName();
            if(username is null) return Result<PhotoDTO>.Failure("username not found");
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user is null) return Result<PhotoDTO>.Failure("user not found to upload its photo");
            var result = await _photoService.AddPhotoAsync(request.File);
            if (result.Error is not null) return Result<PhotoDTO>.Failure(result.Error.Message);

            var photo = new Photo()
            {
                URL = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            user.Photos.Add(photo);

            if (await _unitOfWork.CompleteAsync()) return Result<PhotoDTO>.Success(_mapper.Map<PhotoDTO>(photo));

            return Result<PhotoDTO>.Failure("something went wrong during adding photo in Database"); ;
        }
    }
}
