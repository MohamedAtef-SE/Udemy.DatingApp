using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.DTOs.auth;
using API.Interfaces.Services;
using API.Controllers._common;

namespace API.Controllers
{
    public class AccountController(DataContext dbContext, ITokenService _tokenService) : BaseApiController
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
            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.UserName == loginDTO.Username.ToLower());
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
                Token = _tokenService.GenerateToken(user)
            };
            return Ok(userDTO);
        }


        private async Task<bool> UserExist(string username)
        {
            return await dbContext.Users.AnyAsync(user => user.UserName == username);
        }
    }


}
