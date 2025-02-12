using API.DTOs.members;
using API.Entities;
using API.Helpers;
using API.Pagination;

namespace API.Interfaces.Repositories
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLikeAsync(int sourceUserId, int targetUserId);
        Task<PagedList<MemberDTO>> GetUserLikesAsync(LikesParams likesParams);
        Task<IEnumerable<int>> GetCurrentUserLikeIdsAsync(int currentUserId);
        void DeleteLike(UserLike like);
        Task AddLike(UserLike like);
    }
}
