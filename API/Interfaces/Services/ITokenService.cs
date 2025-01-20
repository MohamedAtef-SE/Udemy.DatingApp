using API.Entities;

namespace API.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(AppUser user);
    }
}
