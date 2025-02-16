using AutoMapper;
using CQRS.Application.DTOs.auth;
using CQRS.Application.Helpers;
using CQRS.Application.Interfaces.Services;
using CQRS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Application._Commands.Account.CreateUser
{
    internal class CreateUserCommandHandler(IMapper _mapper, UserManager<AppUser> _userManager, ITokenService _tokenService) : IRequestHandler<CreateUserCommand, Result<UserDTO>>
    {
        public async Task<Result<UserDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await UserExist(request.RegisterDTO.Username))
                return Result<UserDTO>.Failure("this username is already exist");
            //using var hmac = new HMACSHA512();

            AppUser user = _mapper.Map<AppUser>(request.RegisterDTO);
            user.UserName = request.RegisterDTO.Username.ToLower();
            //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            //user.PasswordSalt = hmac.Key;

            await _userManager.CreateAsync(user, request.RegisterDTO.Password);

            UserDTO userDTO = new()
            {
                Username = user.UserName,
                Token = await _tokenService.GenerateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };

            return Result<UserDTO>.Success(userDTO);
        }



        private async Task<bool> UserExist(string username)
        {
            return await _userManager.Users.AnyAsync(user => user.NormalizedUserName == username.ToUpper());
        }
    }
}
