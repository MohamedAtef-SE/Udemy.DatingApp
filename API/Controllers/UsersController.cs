using API.Controllers._common;
using API.DTOs.members;
using API.Entities;
using API.Extentions;
using API.Helpers;
using API.Interfaces.Repositories;
using API.Interfaces.Services;
using API.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController(IUserRepository _userRepository,IMapper _mapper,IPhotoService _photoService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDTO>>> GetUsers([FromQuery]UserQueryParams userParams)
        {
            var username = User.GetUserName();
            userParams.CurrentUsername = username;
            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader<MemberDTO>(users);

            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDTO>> GetUser(int id)
        {
            var user = await _userRepository.GetMemberByIdAsync(id);

            if (user is null) return NotFound();

            return Ok(user);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            var user = await _userRepository.GetMemberByUsernameAsync(username);

            if (user is null) return NotFound();

            return Ok(user);
        }


        [HttpPut] // PUT: {{baseURL}}/api/Users
        public async Task<ActionResult> UpdateUser(UpdateMemberDTO model)
        {
            var username = User.GetUserName(); // Custom Extention Method.
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user is null) return BadRequest("user not found");

            // with this method user will be updated with no need to call update() just SaveChanges() 
            // because change tracker will feel what updates happend in this user.
            // Update only if values has change else will return to BadRequest("failed to update the user");
            var mappedUser = _mapper.Map(model, user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("failed to update the user");

        }

        [HttpPost("add-photo")] // POST: {{BaseURL}}/api/Users/add-photo
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var username = User.GetUserName();
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user is null) return BadRequest("Cannot find the user to upload photo");
            var result = await _photoService.AddPhotoAsync(file);
            if(result.Error is not null) return BadRequest(result.Error.Message);

            var photo = new Photo()
            {
                URL = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetUser)
                                       , new { username = user.UserName }
                                       ,_mapper.Map<PhotoDTO>(photo));
            }

            return BadRequest("Problem happend during adding the photo");    
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> UpdateUserProfilePicture(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            if (user is null) return BadRequest("Could not find the user");

            var currentPhoto = user.Photos.FirstOrDefault(photo => photo.IsMain);
            var pickedPhoto = user.Photos.FirstOrDefault(photo => photo.Id == photoId);
            if (pickedPhoto is null) return BadRequest("cound not find this photo");

            if (currentPhoto is not null && currentPhoto.Id != pickedPhoto.Id)
            {
                currentPhoto.IsMain = false;
            }

            if(pickedPhoto.IsMain) return NoContent();
            pickedPhoto.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("something happend could not update profile picture");
        }

        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUserName();
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user is null) return BadRequest("user not found");
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo is null) return BadRequest("photo not found");
           if(photo.PublicId is not null)
            {
                var result = await _photoService.DeletePhotoAsycn(photo.PublicId);
                if(result.Error is not null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if(await _userRepository.SaveAllAsync() ) return Ok();
            return BadRequest("failed to delete this photo");
        }

    }
}