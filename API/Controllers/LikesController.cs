using API.Controllers._common;
using API.DTOs.members;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Interfaces;
using API.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController(IUnitOfWork _unitOfWork) : BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult<PagedList<MemberDTO>>> ToggleLike(int targetUserId)
        {
            var soruceUserId = User.GetUserId();
            //PagedList<MemberDTO>? members = null;
            if (soruceUserId == targetUserId) return BadRequest("It's a Good to like yourself,but not in our DatingApp 😏");
            var likeExist = await _unitOfWork.LikesRepository.GetUserLikeAsync(soruceUserId, targetUserId);
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
                    SourceUserId = soruceUserId,
                    TargetUserId = targetUserId
                };
                await _unitOfWork.LikesRepository.AddLike(userLike);
            }

            if (await _unitOfWork.CompleteAsync()) return Ok();

            return BadRequest("failed to update like");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            return Ok(await _unitOfWork.LikesRepository.GetCurrentUserLikeIdsAsync(User.GetUserId()));
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDTO>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _unitOfWork.LikesRepository.GetUserLikesAsync(likesParams);
            Response.AddPaginationHeader<MemberDTO>(users);
            return Ok(users);
        }
    }
}
