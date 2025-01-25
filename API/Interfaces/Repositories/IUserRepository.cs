using API.DTOs.members;
using API.Entities;

namespace API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MemberDTO>> GetMembersAsync();
        Task<MemberDTO?> GetMemberByIdAsync(int id);
        Task<MemberDTO?> GetMemberByUsernameAsync(string username);
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
    }
}
