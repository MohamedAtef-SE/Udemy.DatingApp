using API.Controllers._common;
using API.Extentions;
using CQRS.Application._Commands.Likes.ToggleLike;
using CQRS.Application._Queries.Likes.GetCurrentUserLikeIds;
using CQRS.Application._Queries.Likes.GetUserLikes;
using CQRS.Application.DTOs.members;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController(IMediator _mediator) : BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId)
        {
            var toggleLikeCommand = new ToggleLikeCommand(targetUserId);
            var result = await _mediator.Send(toggleLikeCommand);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            var getCurrentUserLikeIdsQuery = new GetCurrentUserLikeIdsQuery();
            var result = await _mediator.Send(getCurrentUserLikeIdsQuery);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDTO>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            var getUserLikesQuery = new GetUserLikesQuery(likesParams);
            var result = await _mediator.Send(getUserLikesQuery);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            Response.AddPaginationHeader<MemberDTO>(result.Value!);

            return Ok(result.Value);
        }
    }
}
