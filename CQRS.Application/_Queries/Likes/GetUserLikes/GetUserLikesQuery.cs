using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Pagination;
using CQRS.Infrastructure.Params;
using MediatR;

namespace CQRS.Application._Queries.Likes.GetUserLikes
{
    public record GetUserLikesQuery(LikesParams LikesParams) : IRequest<Result<PagedList<MemberDTO>>>;

}
