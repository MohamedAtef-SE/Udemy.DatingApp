using API.Controllers._common;
using API.DTOs.members;
using API.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController(IUserRepository _userRepository,IMapper _mapper) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();

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
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (username == null) return BadRequest("no username found in token");

            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user is null) return BadRequest("user not found");

            // with this method user will be updated with no need to call update() just SaveChanges() 
            // because change tracker will feel what updates happend in this user.
            // Update only if values has change else will return to BadRequest("failed to update the user");
            var mappedUser = _mapper.Map(model, user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("failed to update the user");

        }

    }
}