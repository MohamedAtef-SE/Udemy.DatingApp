using CQRS.Application.DTOs.auth;
using CQRS.Application.Helpers;
using CQRS.Application.Interfaces.Services;
using CQRS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Application._Commands.Account.LoginUser
{
    internal class LoginUserCommandHandler(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, ITokenService _tokenService) : IRequestHandler<LoginUserCommand, Result<UserDTO>>
    {
        public async Task<Result<UserDTO>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            //var user = await _userRepository.GetUserByUsernameAsync(loginDTO.Username);
            var user = await _userManager.Users
                                         .Include(user => user.Photos)
                                         .FirstOrDefaultAsync(user => user.NormalizedUserName == request.LoginDTO.Username.ToUpper());

            if (user is null || user.UserName is null) return Result<UserDTO>.Failure("user not found");

            //using var hmac = new HMACSHA512(user.PasswordSalt);
            //var loggingPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            //for (int i = 0; i < loggingPasswordHash.Length; i++)
            //{
            //    if (loggingPasswordHash[i] != user.PasswordHash[i])
            //    {
            //        return Unauthorized(new { message = "invalid password" });
            //    }
            //}
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.LoginDTO.Password, false);
            // OR
            //var correctedPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            // await _signInManager.SignInAsync(user, false); if you work with cookie base not with JWT
            if (!result.Succeeded) return Result<UserDTO>.Failure("Invalid login");
            UserDTO userDTO = new()
            {
                Username = user.UserName!,
                Token = await _tokenService.GenerateToken(user),
                PhotoURL = user.Photos.FirstOrDefault(photo => photo.IsMain)?.URL,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };

            return Result<UserDTO>.Success(userDTO);
        }
    }
}
