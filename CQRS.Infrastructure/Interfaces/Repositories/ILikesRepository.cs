using CQRS.Application.DTOs.members;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;

namespace CQRS.Infrastructure.Interfaces.Repositories
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLikeAsync(int? sourceUserId, int targetUserId);
        Task<PagedList<MemberDTO>> GetUserLikesAsync(LikesParams likesParams);
        Task<IEnumerable<int>?> GetCurrentUserLikeIdsAsync(int currentUserId);
        void DeleteLike(UserLike like);
        Task AddLike(UserLike like);
    }
}
