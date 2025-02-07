using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(AppUser user);
    }
}
