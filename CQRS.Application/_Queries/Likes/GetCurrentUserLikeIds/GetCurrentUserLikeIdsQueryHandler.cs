using CQRS.Application.Helpers;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Queries.Likes.GetCurrentUserLikeIds
{
    internal class GetCurrentUserLikeIdsQueryHandler(IHttpContextAccessor _httpContextAccessor, IUnitOfWork _unitOfWork) : IRequestHandler<GetCurrentUserLikeIdsQuery, Result<IEnumerable<int>>>
    {
        public async Task<Result<IEnumerable<int>>> Handle(GetCurrentUserLikeIdsQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (userId is null) return Result<IEnumerable<int>>.Failure("user id not found");

            var userLikeIds = await _unitOfWork.LikesRepository.GetCurrentUserLikeIdsAsync(userId.Value);

            if (userLikeIds is not null) return Result<IEnumerable<int>>.Success(userLikeIds);

            throw new Exception("somehing went wrong during fetching user like ids from database.");
        }
    }
}