using API.Controllers._common;
using API.DTOs.auth;
using API.Entities;
using API.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, ITokenService _tokenService, IMapper _mapper) : BaseApiController
    {
        [HttpPost("register")] // {{BaseURL}}/api/account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExist(registerDTO.Username))
                return BadRequest("this username is already exist");

            //using var hmac = new HMACSHA512();

            AppUser user = _mapper.Map<AppUser>(registerDTO);
            user.UserName = registerDTO.Username.ToLower();
            //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            //user.PasswordSalt = hmac.Key;

            await _userManager.CreateAsync(user, registerDTO.Password);

            UserDTO userDTO = new()
            {
                Username = user.UserName,
                Token = await _tokenService.GenerateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };

            return Ok(userDTO);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {

            //var user = await _userRepository.GetUserByUsernameAsync(loginDTO.Username);
            var user = await _userManager.Users
                                         .Include(user => user.Photos)
                                         .FirstOrDefaultAsync(user => user.NormalizedUserName == loginDTO.Username.ToUpper());

            if (user is null || user.UserName is null) return Unauthorized(new { message = "invalid username" });

            //using var hmac = new HMACSHA512(user.PasswordSalt);
            //var loggingPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            //for (int i = 0; i < loggingPasswordHash.Length; i++)
            //{
            //    if (loggingPasswordHash[i] != user.PasswordHash[i])
            //    {
            //        return Unauthorized(new { message = "invalid password" });
            //    }
            //}
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            // OR
            //var correctedPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            // await _signInManager.SignInAsync(user, false); if you work with cookie base not with JWT
            if (!result.Succeeded) return BadRequest("Invalid login");
            UserDTO userDTO = new()
            {
                Username = user.UserName!,
                Token = await _tokenService.GenerateToken(user),
                PhotoURL = user.Photos.FirstOrDefault(photo => photo.IsMain)?.URL,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
            return Ok(userDTO);
        }


        private async Task<bool> UserExist(string username)
        {
            return await _userManager.Users.AnyAsync(user => user.NormalizedUserName == username.ToUpper());
        }
    }


}
