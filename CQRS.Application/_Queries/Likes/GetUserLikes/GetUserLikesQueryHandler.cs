using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Extensions;
using CQRS.Infrastructure.Interfaces;
using CQRS.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CQRS.Application._Queries.Likes.GetUserLikes
{
    public class GetUserLikesQueryHandler(IUnitOfWork _unitOfWork,IHttpContextAccessor _httpContextAccessor) : IRequestHandler<GetUserLikesQuery, Result<PagedList<MemberDTO>>>
    {
        public async Task<Result<PagedList<MemberDTO>>> Handle(GetUserLikesQuery request, CancellationToken cancellationToken)
        {
            request.LikesParams.UserId = _httpContextAccessor.HttpContext?.User.GetUserId() ?? 0;

            if (request.LikesParams.UserId == 0)
                return Result<PagedList<MemberDTO>>.Failure("failed to get user id");

            var users = await _unitOfWork.LikesRepository.GetUserLikesAsync(request.LikesParams);
            if (users is not null) return Result<PagedList<MemberDTO>>.Success(users);

            throw new Exception("somehing went wrong during fetching users likes from database.");
        }
    }
}
