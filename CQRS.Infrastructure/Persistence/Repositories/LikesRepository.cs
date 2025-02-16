using AutoMapper;
using AutoMapper.QueryableExtensions;
using CQRS.Application.DTOs.members;
using CQRS.Domain.Entities;
using CQRS.Infrastructure.Interfaces.Repositories;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Infrastructure.Persistence.Repositories
{
    public class LikesRepository(DataContext _dbContext, IMapper _mapper) : ILikesRepository
    {
        public async Task<IEnumerable<int>?> GetCurrentUserLikeIdsAsync(int currentUserId)
        {
            var userIdsLikedByCurrentUser = await _dbContext.Likes.Where(ul => ul.SourceUserId == currentUserId)
                                                                 .Select(ul => ul.TargetUserId)
                                                                 .ToListAsync();
            return userIdsLikedByCurrentUser;
        }

        public async Task<UserLike?> GetUserLikeAsync(int? sourceUserId, int targetUserId)
        {
            return await _dbContext.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<MemberDTO>> GetUserLikesAsync(LikesParams likesParams)
        {
            switch (likesParams.Predicate)
            {
                case "liked":
                    var targetMembers = _dbContext.Likes.Where(ul => ul.SourceUserId == likesParams.UserId)
                                   .Select(ul => ul.TargetUser)
                                   .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider);
                    return await PagedList<MemberDTO>.CreateAsync(targetMembers, likesParams.PageNumber, likesParams.PageSize);


                case "likedBy":
                    var sourceMembers = _dbContext.Likes.Where(ul => ul.TargetUserId == likesParams.UserId)
                                   .Select(ul => ul.SourceUser)
                                   .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider);
                    return await PagedList<MemberDTO>.CreateAsync(sourceMembers, likesParams.PageNumber, likesParams.PageSize);
                default:
                    var userIdsLikedByCurrentUser = await GetCurrentUserLikeIdsAsync(likesParams.UserId);
                    var mutualMembers = _dbContext.Likes
                                            .Where(ul => ul.TargetUserId == likesParams.UserId
                                                   &&
                                                  userIdsLikedByCurrentUser!.Contains(ul.SourceUserId))
                                            .Select(ul => ul.SourceUser)
                                            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider);
                    return await PagedList<MemberDTO>.CreateAsync(mutualMembers, likesParams.PageNumber, likesParams.PageSize);

            }
        }
        public async Task AddLike(UserLike like)
        {
            await _dbContext.Likes.AddAsync(like);
        }

        public void DeleteLike(UserLike like)
        {
            _dbContext.Likes.Remove(like);
        }
    }
}
