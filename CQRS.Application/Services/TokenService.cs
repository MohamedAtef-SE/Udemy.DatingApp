using CQRS.Application.Helpers;
using CQRS.Application.Interfaces.Services;
using CQRS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CQRS.Application.Services
{
    public class TokenService(IOptions<JWTSettings> jwtSettings, UserManager<AppUser> _userManager) : ITokenService
    {
        public async Task<string> GenerateToken(AppUser user)
        {

            string securityKey = jwtSettings.Value.SecurityKey ?? throw new Exception("SecurityKey is not found in appSettings.json");
            if (securityKey.Length < 64) throw new Exception("SecurityKey must to be longer");
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey,
                                                            SecurityAlgorithms.HmacSha512Signature);

            List<Claim> privateClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName!)
            };

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            privateClaims.AddRange(roleClaims);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(privateClaims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);
            return token;

        }
    }
}
