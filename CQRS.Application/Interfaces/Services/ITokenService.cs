using CQRS.Domain.Entities;

namespace CQRS.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(AppUser user);
    }
}
