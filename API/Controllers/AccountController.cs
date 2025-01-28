using API.Controllers._common;
using API.Data;
using API.DTOs.auth;
using API.Interfaces.Repositories;
using API.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DataContext dbContext,IUserRepository _userRepository, ITokenService _tokenService) : BaseApiController
    {
        [HttpPost("register")] // {{BaseURL}}/api/account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExist(registerDTO.Username)) return BadRequest("this username is already exist");

            return Ok();
            //using var hmac = new HMACSHA512();

            //AppUser user = new AppUser()
            //{
            //    UserName = registerDTO.Username.ToLower(),
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            //    PasswordSalt = hmac.Key
            //};

            //await dbContext.Users.AddAsync(user);
            //await dbContext.SaveChangesAsync();

            //UserDTO userDTO = new()
            //{
            //    Username = user.UserName,
            //    Token = _tokenService.GenerateToken(user)
            //};

            //return Ok(userDTO);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDTO.Username);
            if (user is null) return Unauthorized(new { message = "invalid username" });

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var loggingPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i < loggingPasswordHash.Length; i++)
            {
                if (loggingPasswordHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized(new { message = "invalid password" });
                }
            }

            UserDTO userDTO = new()
            {
                Username = user.UserName,
                Token = _tokenService.GenerateToken(user),
                PhotoURL = user.Photos.FirstOrDefault(photo => photo.IsMain)?.URL
            };
            return Ok(userDTO);
        }


        private async Task<bool> UserExist(string username)
        {
            return await dbContext.Users.AnyAsync(user => user.UserName == username);
        }
    }


}
