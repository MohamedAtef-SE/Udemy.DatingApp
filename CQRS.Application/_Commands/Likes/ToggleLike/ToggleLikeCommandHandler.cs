using CQRS.Application.Helpers;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Commands.Likes.ToggleLike
{
    public class ToggleLikeCommandHandler(IHttpContextAccessor _httpContextAccessor,IUnitOfWork _unitOfWork) : IRequestHandler<ToggleLikeCommand,Result<bool>>
    {
        public async Task<Result<bool>> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
        {
            var soruceUserId =  _httpContextAccessor?.HttpContext?.User.GetUserId();
            //PagedList<MemberDTO>? members = null;
            if (soruceUserId == request.TargetUserId) return Result<bool>.Failure("It's a Good to like yourself,but not in our DatingApp 😏");
            if (soruceUserId is null) return Result<bool>.Failure("source userId is not found");

            var likeExist = await _unitOfWork.LikesRepository.GetUserLikeAsync(soruceUserId, request.TargetUserId);
            if (likeExist is not null)
            {
                _unitOfWork.LikesRepository.DeleteLike(likeExist);
                //var likesParams = new LikesParams() { Predicate = "liked" };
                //likesParams.UserId = soruceUserId;
                //if (await _likesRepository.SaveChangesAsync())
                //{
                //    members = await _likesRepository.GetUserLikesAsync(likesParams);
                //    Response.AddPaginationHeader<MemberDTO>(members);
                //    return Ok(members);
                //}
            }
            else
            {
                var userLike = new UserLike()
                {
                    SourceUserId = soruceUserId.Value,
                    TargetUserId = request.TargetUserId,
                };
                await _unitOfWork.LikesRepository.AddLike(userLike);
            }

            return Result<bool>.Success(await _unitOfWork.CompleteAsync());
        }
    }
}
