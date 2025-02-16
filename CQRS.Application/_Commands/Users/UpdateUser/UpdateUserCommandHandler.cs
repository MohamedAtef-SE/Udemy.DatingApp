using AutoMapper;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Commands.Users.UpdateUser
{
    public class UpdateUserCommandHandler(IHttpContextAccessor _httpContextAccessor,IUnitOfWork _unitOfWork,IMapper _mapper) : IRequestHandler<UpdateUserCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            var username = _httpContextAccessor.HttpContext?.User.GetUserName(); // Custom Extention Method.
            if(username is null) return Result<bool>.Failure("username not found");
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (user is null) return Result<bool>.Failure("user not found");

            // with this method user will be updated with no need to call update() just SaveChanges() 
            // because change tracker will feel what updates happend in this user.
            // Update only if values has change else will return to BadRequest("failed to update the user");
            var mappedUser = _mapper.Map(request.UpdateMemberDTO, user);

            if (await _unitOfWork.CompleteAsync()) return Result<bool>.Success(true);

            return Result<bool>.Failure("something went wrong during complete updating user in database");
        }
    }
}
