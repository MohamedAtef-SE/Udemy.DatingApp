using API.Controllers._common;
using API.Extentions;
using CQRS.Application._Commands.Users.AddPhoto;
using CQRS.Application._Commands.Users.DeletePhoto;
using CQRS.Application._Commands.Users.UpdateUser;
using CQRS.Application._Commands.Users.UpdateUserProfilePicture;
using CQRS.Application._Queries.Users.GetUserById;
using CQRS.Application._Queries.Users.GetUserByUsername;
using CQRS.Application._Queries.Users.GetUsers;
using CQRS.Application.DTOs.members;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController(IMediator _mediator) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDTO>>> GetUsers([FromQuery] UserQueryParams UserQueryParams)
        {
            var usersQuery = new GetUsersQuery(UserQueryParams);
            var result = await _mediator.Send(usersQuery);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            Response.AddPaginationHeader<MemberDTO>(result.Value!);

            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDTO>> GetUser(int id)
        {
            var userByIdQuery = new GetUserByIdQuery(id);
            var result = await _mediator.Send(userByIdQuery);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            var userByUsernameQuery = new GetUserByUsernameQuery(username);
           var result = await _mediator.Send(userByUsernameQuery);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPut] // PUT: {{baseURL}}/api/Users
        public async Task<ActionResult> UpdateUser(UpdateMemberDTO updateMemberDTO)
        {
            var updateUserCommand = new UpdateUserCommand(updateMemberDTO);

            var result = await _mediator.Send(updateUserCommand);

            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return NoContent();

        }

        [HttpPost("add-photo")] // POST: {{BaseURL}}/api/Users/add-photo
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var addPhotoCommand = new AddPhotoCommand(file);
            var result = await _mediator.Send(addPhotoCommand);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetUser), new { username = User.GetUserName() }, result.Value);
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> UpdateUserProfilePicture(int photoId)
        {
            var updateUserProfilePictureCommand = new UpdateUserProfilePictureCommand(photoId);
            var result = await _mediator.Send(updateUserProfilePictureCommand);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var deletePhotoCommand = new DeletePhotoCommand(photoId);
            var result = await _mediator.Send(deletePhotoCommand);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(); 
        }

    }
}