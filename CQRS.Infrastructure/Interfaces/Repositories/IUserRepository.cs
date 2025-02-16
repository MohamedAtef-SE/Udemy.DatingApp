using CQRS.Application.DTOs.members;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;

namespace CQRS.Infrastructure.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<PagedList<MemberDTO>> GetMembersAsync(UserQueryParams userParams);
        Task<MemberDTO?> GetMemberByIdAsync(int id);
        Task<MemberDTO?> GetMemberByUsernameAsync(string username);
        void Update(AppUser user);
    }
}
